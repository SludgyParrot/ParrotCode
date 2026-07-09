/*

Parrot Code
Copyright (c) 2026 Sludgy Parrot (Pty) Ltd. All Rights Reserved.

This source code is proprietary and confidential software owned by
Sludgy Parrot (Pty) Ltd.

Parrot Code is a commercial software product developed and distributed
by Sludgy Parrot (Pty) Ltd.

Unauthorized copying, modification, distribution, sublicensing,
reverse engineering, decompilation, disclosure, or use of this
software, in whole or in part, is strictly prohibited without
prior written permission from Sludgy Parrot (Pty) Ltd.

This software is provided under the terms of a separate license
agreement. Possession of this source code does not grant any rights
to use, modify, distribute, or create derivative works unless
explicitly authorized by a valid written license.

THE SOFTWARE IS PROVIDED "AS IS" WITHOUT WARRANTY OF ANY KIND,
EXPRESS OR IMPLIED, EXCEPT AS REQUIRED BY APPLICABLE LAW.

For licensing inquiries:
licensing@sludgyparrot.com

*/

#region Included System Assemblies
using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
#endregion

#region Included Unity Assemblies
using UnityEngine;
using UnityEditor;
#endregion

#region Included Jet Brains Assemblies
using JetBrains.Annotations;
#endregion

namespace ParrotCode.Helpers.Storage
{
    /// <summary>
    /// Provides helper methods for copying files and directories.
    /// </summary>
    /// <remarks>
    /// This class contains high-level storage utilities for recursively copying
    /// directory contents while optionally excluding specific folders and file
    /// extensions.
    /// </remarks>
    public static class Storage
    {
        /// <summary>
        /// Serializes an object to JSON and writes it to the specified file.
        /// </summary>
        /// <typeparam name="T">
        /// The reference type to serialize.
        /// </typeparam>
        /// <param name="path">
        /// The full path of the JSON file to create or overwrite.
        /// If the parent directory does not exist, it is created automatically.
        /// </param>
        /// <param name="content">
        /// The object to serialize and write to the file.
        /// </param>
        /// <param name="encoding">
        /// The text encoding used to write the file.
        /// If <see langword="null"/>, <see cref="Encoding.UTF8"/> is used.
        /// </param>
        /// <exception cref="ArgumentException">
        /// Thrown if <paramref name="path"/> is null, empty, consists only of
        /// white-space characters, or does not contain a valid directory.
        /// </exception>
        /// <exception cref="ArgumentNullException">
        /// Thrown if <paramref name="content"/> is <see langword="null"/>.
        /// </exception>
        /// <exception cref="InvalidDataException">
        /// Thrown if the object could not be serialized into a valid JSON string.
        /// </exception>
        /// <remarks>
        /// This method uses <see cref="EditorJsonUtility"/> to serialize the object,
        /// producing indented (pretty-printed) JSON output. Existing files are
        /// overwritten.
        /// </remarks>
        public static void SerializeToJsonFile<T>(
            string path,
            T content,
            [CanBeNull] Encoding encoding = null)
            where T : class
        {
            if (string.IsNullOrWhiteSpace(path))
            {
                throw new ArgumentException("Path cannot be null or empty.", nameof(path));
            }

            if (content == null)
            {
                throw new ArgumentNullException(nameof(content), "Content cannot be null.");
            }

            string? directory = Path.GetDirectoryName(path);

            if (string.IsNullOrEmpty(directory))
            {
                throw new ArgumentException(
                    $"The path '{path}' must include a valid directory.",
                    nameof(path));
            }

            Directory.CreateDirectory(directory);

            string jsonString = EditorJsonUtility.ToJson(content);

            if (string.IsNullOrWhiteSpace(jsonString))
            {
                throw new InvalidDataException(
                    $"Couldn't create a JSON file at path: {path}");
            }

            Encoding encodingType = encoding ?? Encoding.UTF8;
            File.WriteAllText(path, jsonString, encodingType);
        }

        /// <summary>
        /// Deserializes a JSON file into an instance of the specified type.
        /// </summary>
        /// <typeparam name="T">
        /// The reference type to deserialize.
        /// </typeparam>
        /// <param name="path">
        /// The full path of the JSON file.
        /// </param>
        /// <returns>
        /// An instance of <typeparamref name="T"/> populated with the data
        /// contained in the JSON file.
        /// </returns>
        /// <exception cref="ArgumentException">
        /// Thrown if <paramref name="path"/> is null, empty, or consists only
        /// of white-space characters.
        /// </exception>
        /// <exception cref="FileNotFoundException">
        /// Thrown if the specified file does not exist.
        /// </exception>
        /// <exception cref="InvalidDataException">
        /// Thrown if the JSON file could not be read or contains no data.
        /// </exception>
        /// <exception cref="InvalidOperationException">
        /// Thrown if the JSON data could not be deserialized into
        /// <typeparamref name="T"/>.
        /// </exception>
        /// <remarks>
        /// This method uses <see cref="EditorJsonUtility"/> to deserialize the
        /// JSON content.
        /// </remarks>
        public static T DeserializeFromJsonFile<T>(string path) where T : class, new()
        {
            if (string.IsNullOrWhiteSpace(path))
            {
                throw new ArgumentException("Path cannot be null or empty.", nameof(path));
            }

            if (!File.Exists(path))
            {
                throw new ArgumentException(
                    $"The path '{path}' must include a valid directory.",
                    nameof(path));
            }

            string jsonString = File.ReadAllText(path);

            if (string.IsNullOrWhiteSpace(jsonString))
            {
                throw new InvalidDataException(
                    $"Couldn't read a JSON file at path: {path}");
            }

            T data = new T();

            EditorJsonUtility.FromJsonOverwrite(jsonString, data);

            if(data == null)
            {
                throw new InvalidOperationException($"Failed to load {typeof(T)} " +
                    $"from a json file: {jsonString} at path: {path}");
            }

            return data;
        }

        /// <summary>
        /// Recursively copies the contents of a directory to the specified destination.
        /// </summary>
        /// <param name="source">
        /// The path of the source directory to copy.
        /// </param>
        /// <param name="destination">
        /// The path of the destination directory.
        /// </param>
        /// <param name="excludedFoldersAndFileExtensions">
        /// An optional collection of folder names and file extensions to exclude
        /// from the copy operation. File extensions should include the leading
        /// period (for example, <c>".meta"</c> or <c>".tmp"</c>).
        /// Folder exclusions are matched by directory name.
        /// </param>
        /// <exception cref="ArgumentException">
        /// Thrown when <paramref name="source"/> or <paramref name="destination"/>
        /// is <see langword="null"/>, empty, or consists only of white-space characters.
        /// </exception>
        /// <exception cref="DirectoryNotFoundException">
        /// Thrown when the source directory does not exist.
        /// </exception>
        public static void Copy(string source, string destination, params string[] excludedFoldersAndFileExtensions)
        {
            if(string.IsNullOrWhiteSpace(source))
            {
                throw new ArgumentException("Source directory cannot be null, empty," +
                    " or consist entirely of white space.", nameof(source));
            }

            if(string.IsNullOrWhiteSpace(destination))
            {
                throw new ArgumentException("Source destination cannot be null, empty," +
                    " or consist entirely of white space.", nameof(destination));
            }

            if(!Directory.Exists(source))
            {
                throw new DirectoryNotFoundException(source);
            }

            HashSet<string> excluded = 
                new HashSet<string>(excludedFoldersAndFileExtensions,
                StringComparer.OrdinalIgnoreCase);

            CopyInternal(source, destination, excluded);
        }

        #region Internal

        /// <summary>
        /// Recursively copies the contents of a directory while applying the specified
        /// exclusion rules.
        /// </summary>
        /// <param name="source">
        /// The path of the source directory.
        /// </param>
        /// <param name="destination">
        /// The path of the destination directory.
        /// </param>
        /// <param name="excluded">
        /// A collection containing folder names and file extensions to exclude from
        /// the copy operation.
        /// </param>
        private static void CopyInternal(string source, string destination, HashSet<string> excluded)
        {
            UnityEngine.Debug.Log($"Copying files from: {source} to {destination}");

            DirectoryInfo directoryInfo = new DirectoryInfo(source);

            if (!Directory.Exists(destination))
            {
                Directory.CreateDirectory(destination);
            }

            foreach (DirectoryInfo dir in directoryInfo.GetDirectories())
            {
                if (excluded.Contains(dir.Name))
                    continue;

                CopyInternal(dir.FullName, Path.Combine(destination, dir.Name), excluded);
            }

            foreach (FileInfo file in directoryInfo.GetFiles())
            {
                if (excluded.Contains(file.Extension))
                    continue;

                file.CopyTo(Path.Combine(destination, file.Name), true);
            }
        }

        #endregion
    }
}
