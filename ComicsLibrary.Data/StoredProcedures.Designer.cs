﻿//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated by a tool.
//     Runtime Version:4.0.30319.42000
//
//     Changes to this file may cause incorrect behavior and will be lost if
//     the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace ComicsLibrary.Data {
    using System;
    
    
    /// <summary>
    ///   A strongly-typed resource class, for looking up localized strings, etc.
    /// </summary>
    // This class was auto-generated by the StronglyTypedResourceBuilder
    // class via a tool like ResGen or Visual Studio.
    // To add or remove a member, edit your .ResX file then rerun ResGen
    // with the /str option, or rebuild your VS project.
    [global::System.CodeDom.Compiler.GeneratedCodeAttribute("System.Resources.Tools.StronglyTypedResourceBuilder", "16.0.0.0")]
    [global::System.Diagnostics.DebuggerNonUserCodeAttribute()]
    [global::System.Runtime.CompilerServices.CompilerGeneratedAttribute()]
    internal class StoredProcedures {
        
        private static global::System.Resources.ResourceManager resourceMan;
        
        private static global::System.Globalization.CultureInfo resourceCulture;
        
        [global::System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1811:AvoidUncalledPrivateCode")]
        internal StoredProcedures() {
        }
        
        /// <summary>
        ///   Returns the cached ResourceManager instance used by this class.
        /// </summary>
        [global::System.ComponentModel.EditorBrowsableAttribute(global::System.ComponentModel.EditorBrowsableState.Advanced)]
        internal static global::System.Resources.ResourceManager ResourceManager {
            get {
                if (object.ReferenceEquals(resourceMan, null)) {
                    global::System.Resources.ResourceManager temp = new global::System.Resources.ResourceManager("ComicsLibrary.Data.StoredProcedures", typeof(StoredProcedures).Assembly);
                    resourceMan = temp;
                }
                return resourceMan;
            }
        }
        
        /// <summary>
        ///   Overrides the current thread's CurrentUICulture property for all
        ///   resource lookups using this strongly typed resource class.
        /// </summary>
        [global::System.ComponentModel.EditorBrowsableAttribute(global::System.ComponentModel.EditorBrowsableState.Advanced)]
        internal static global::System.Globalization.CultureInfo Culture {
            get {
                return resourceCulture;
            }
            set {
                resourceCulture = value;
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to CREATE PROCEDURE [ComicsLibrary].[GetAllSeries]
        ///AS
        ///BEGIN
        ///
        ///	SELECT 
        ///		S.Id,
        ///        S.Title,
        ///        B.ImageUrl,
        ///        (SELECT COUNT([BookId]) FROM [ComicsLibrary].[SeriesUnreadBooks] WHERE [SeriesId] = S.[Id]) UnreadBooks,
        ///        (SELECT COUNT([BookId]) FROM [ComicsLibrary].[SeriesAllBooks] WHERE [SeriesId] = S.[Id]) TotalBooks,
        ///        S.SourceID,
        ///        SRC.[Name] SourceName,
        ///        [StartYear],
        ///        [EndYear],
        ///        [LastUpdated],
        ///        [Abandoned] [Archived],
        ///        S.[Sourc [rest of string was truncated]&quot;;.
        /// </summary>
        internal static string Create_GetAllSeries {
            get {
                return ResourceManager.GetString("Create_GetAllSeries", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to CREATE PROCEDURE [ComicsLibrary].[GetAllSeries]
        ///    @SeriesID INT = NULL
        ///AS
        ///BEGIN
        ///
        ///	SELECT 
        ///		S.Id,
        ///        S.Title,
        ///        B.ImageUrl,
        ///        (SELECT COUNT([BookId]) FROM [ComicsLibrary].[SeriesUnreadBooks] WHERE [SeriesId] = S.[Id]) UnreadBooks,
        ///        (SELECT COUNT([BookId]) FROM [ComicsLibrary].[SeriesAllBooks] WHERE [SeriesId] = S.[Id]) TotalBooks,
        ///        [Abandoned] [Archived]
        ///	FROM [ComicsLibrary].[Series] S
        ///		INNER JOIN [ComicsLibrary].[SeriesAllBooks] U ON U.[SeriesId] = S.[Id]
        ///		 [rest of string was truncated]&quot;;.
        /// </summary>
        internal static string Create_GetAllSeries_v2 {
            get {
                return ResourceManager.GetString("Create_GetAllSeries_v2", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to CREATE PROCEDURE [ComicsLibrary].[GetHomeBooks]
        ///AS
        ///BEGIN
        ///
        ///	WITH [ReadableBooks] AS (
        ///		SELECT 
        ///			S.[Id] [SeriesId], 
        ///			B.[Id] [BookId],
        ///			B.[Title], 
        ///			B.[Number], 
        ///			B.[ImageUrl],
        ///			T.[Name] [BookTypeName],
        ///			T.[ID] [BookTypeID],
        ///			DENSE_RANK() OVER (PARTITION BY S.[Id] ORDER BY B.[OnSaleDate], B.[Number], B.[SourceItemID]) [Rank]
        ///		FROM [ComicsLibrary].[Series] S
        ///			INNER JOIN [ComicsLibrary].[Books] B ON B.[SeriesId] = S.[Id]
        ///				AND B.[Hidden] = 0
        ///				AND B.[DateRead] IS NULL
        ///	 [rest of string was truncated]&quot;;.
        /// </summary>
        internal static string Create_GetHomeBooks_v1 {
            get {
                return ResourceManager.GetString("Create_GetHomeBooks_v1", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to CREATE PROCEDURE [ComicsLibrary].[GetHomeBooks]
        ///AS
        ///BEGIN
        ///
        ///		WITH [ReadableBooks] AS (
        ///		SELECT 
        ///			S.[Id] [SeriesId], 
        ///			B.[Id] [BookId],
        ///			B.[Title], 
        ///			B.[Number], 
        ///			B.[ImageUrl],
        ///			B.[ReadUrl],
        ///			B.[Creators],
        ///			T.[Name] [BookTypeName],
        ///			T.[ID] [BookTypeID],
        ///			DENSE_RANK() OVER (PARTITION BY S.[Id] ORDER BY B.[OnSaleDate], B.[Number], B.[SourceItemID]) [Rank]
        ///		FROM [ComicsLibrary].[Series] S
        ///			INNER JOIN [ComicsLibrary].[Books] B ON B.[SeriesId] = S.[Id]
        ///				AND B.[Hidden]  [rest of string was truncated]&quot;;.
        /// </summary>
        internal static string Create_GetHomeBooks_v2 {
            get {
                return ResourceManager.GetString("Create_GetHomeBooks_v2", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to CREATE PROCEDURE [ComicsLibrary].[GetHomeBooks]
        ///	@SeriesID INT = NULL
        ///AS
        ///BEGIN
        ///
        ///		WITH [ReadableBooks] AS (
        ///		SELECT 
        ///			S.[Id] [SeriesId], 
        ///			B.[Id] [BookId],
        ///			B.[Title], 
        ///			B.[Number], 
        ///			B.[ImageUrl],
        ///			B.[ReadUrl],
        ///			B.[Creators],
        ///			T.[Name] [BookTypeName],
        ///			T.[ID] [BookTypeID],
        ///			DENSE_RANK() OVER (PARTITION BY S.[Id] ORDER BY B.[OnSaleDate], B.[Number], B.[SourceItemID]) [Rank]
        ///		FROM [ComicsLibrary].[Series] S
        ///			INNER JOIN [ComicsLibrary].[Books] B ON B.[SeriesId] = S.[I [rest of string was truncated]&quot;;.
        /// </summary>
        internal static string Create_GetHomeBooks_v3 {
            get {
                return ResourceManager.GetString("Create_GetHomeBooks_v3", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to CREATE PROCEDURE [ComicsLibrary].[GetHomeBooks]
        ///	@SeriesID INT = NULL
        ///AS
        ///BEGIN
        ///
        ///	SELECT 
        ///		 B.[Id]
        ///		  ,S.[Id] SeriesId
        ///		  ,s.Title SeriesTitle
        ///		  ,CASE
        ///			WHEN b.BookTypeID = 1 THEN CONCAT(&apos;#&apos;, CAST(b.Number AS VARCHAR))
        ///			WHEN b.BookTypeID = 2 THEN CONCAT(&apos;Vol. &apos;, CAST(b.Number AS VARCHAR))
        ///			ELSE &apos;&apos;
        ///			END IssueTitle
        ///		  ,b.ImageUrl
        ///		  ,b.ReadUrl
        ///		  ,(SELECT COUNT(BookId) FROM [ComicsLibrary].[SeriesUnreadBooks] WHERE SeriesId = S.Id) UnreadIssues
        ///		  ,b.Creators
        ///	FROM [ComicsLibr [rest of string was truncated]&quot;;.
        /// </summary>
        internal static string Create_GetHomeBooks_v4 {
            get {
                return ResourceManager.GetString("Create_GetHomeBooks_v4", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to IF EXISTS (
        ///	SELECT object_id 
        ///	FROM SYS.OBJECTS O
        ///		INNER JOIN SYS.SCHEMAS S ON S.schema_id = O.schema_id
        ///	WHERE O.[Name] = &apos;GetAllSeries&apos;
        ///		AND S.[Name] = &apos;ComicsLibrary&apos;
        ///)
        ///BEGIN
        ///	DROP PROCEDURE [ComicsLibrary].[GetAllSeries]
        ///END
        ///GO.
        /// </summary>
        internal static string Drop_GetAllSeries {
            get {
                return ResourceManager.GetString("Drop_GetAllSeries", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to IF EXISTS (
        ///	SELECT object_id 
        ///	FROM SYS.OBJECTS O
        ///		INNER JOIN SYS.SCHEMAS S ON S.schema_id = O.schema_id
        ///	WHERE O.[Name] = &apos;GetHomeBooks&apos;
        ///		AND S.[Name] = &apos;ComicsLibrary&apos;
        ///)
        ///BEGIN
        ///	DROP PROCEDURE [ComicsLibrary].[GetHomeBooks]
        ///END
        ///GO.
        /// </summary>
        internal static string Drop_GetHomeBooks {
            get {
                return ResourceManager.GetString("Drop_GetHomeBooks", resourceCulture);
            }
        }
    }
}
