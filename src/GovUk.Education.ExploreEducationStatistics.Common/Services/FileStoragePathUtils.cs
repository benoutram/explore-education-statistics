using System;
using GovUk.Education.ExploreEducationStatistics.Common.Extensions;
using GovUk.Education.ExploreEducationStatistics.Common.Model;
using static GovUk.Education.ExploreEducationStatistics.Common.Model.FileType;

namespace GovUk.Education.ExploreEducationStatistics.Common.Services
{
    public static class FileStoragePathUtils
    {
        public static string PublicContentStagingPath()
        {
            return "staging";
        }

        private static string PublicContentDownloadPath(string prefix = null)
        {
            return $"{AppendPathSeparator(prefix)}download";
        }

        private static string PublicContentFastTrackPath(string prefix = null)
        {
            return $"{AppendPathSeparator(prefix)}fast-track";
        }

        private static string PublicContentMethodologiesPath(string prefix = null)
        {
            return $"{AppendPathSeparator(prefix)}methodology";
        }

        private static string PublicContentPublicationsPath(string prefix = null)
        {
            return $"{AppendPathSeparator(prefix)}publications";
        }

        public static string PublicContentDownloadTreePath(string prefix = null)
        {
            return $"{PublicContentDownloadPath(prefix)}/tree.json";
        }

        public static string PublicContentReleaseFastTrackPath(string releaseId, string prefix = null)
        {
            return $"{PublicContentFastTrackPath(prefix)}/{releaseId}";
        }

        public static string PublicContentFastTrackPath(string releaseId, string id, string prefix = null)
        {
            return $"{PublicContentReleaseFastTrackPath(releaseId, prefix)}/{id}.json";
        }

        public static string PublicContentMethodologyTreePath(string prefix = null)
        {
            return $"{PublicContentMethodologiesPath(prefix)}/tree.json";
        }

        public static string PublicContentPublicationsTreePath(string prefix = null)
        {
            return $"{PublicContentPublicationsPath(prefix)}/tree.json";
        }

        public static string PublicContentMethodologyPath(string slug, string prefix = null)
        {
            return $"{PublicContentMethodologiesPath(prefix)}/methodologies/{slug}.json";
        }

        public static string PublicContentPublicationParentPath(string slug, string prefix = null)
        {
            return $"{PublicContentPublicationsPath(prefix)}/{slug}";
        }

        public static string PublicContentReleaseParentPath(string publicationSlug, string releaseSlug, string prefix = null)
        {
            return $"{PublicContentPublicationParentPath(publicationSlug, prefix)}/releases/{releaseSlug}";
        }

        public static string PublicContentPublicationPath(string slug, string prefix = null)
        {
            return $"{PublicContentPublicationParentPath(slug, prefix)}/publication.json";
        }

        public static string PublicContentLatestReleasePath(string slug, string prefix = null)
        {
            return $"{PublicContentPublicationParentPath(slug, prefix)}/latest-release.json";
        }

        public static string PublicContentReleasePath(string publicationSlug, string releaseSlug, string prefix = null)
        {
            return $"{PublicContentPublicationParentPath(publicationSlug, prefix)}/releases/{releaseSlug}.json";
        }

        public static string PublicContentDataBlockPath(
            string publicationSlug,
            string releaseSlug,
            Guid dataBlockId,
            string prefix = null)
        {
            return $"{PublicContentReleaseParentPath(publicationSlug, releaseSlug, prefix)}/data-blocks/{dataBlockId}.json";
        }

        private static FileType GetUploadFolderForType(FileType type)
        {
            return type == Metadata ? Data : type;
        }

        /**
         * The admin directory path where data files are batched for importing
         */
        public static string AdminReleaseBatchesDirectoryPath(Guid blobPath, Guid fileId)
        {
            return $"{blobPath}/{Data.GetEnumLabel()}/batches/{fileId}/";
        }

        /**
         * The admin file path, for a file of a particular type and id, on a release.
         */
        public static string AdminReleasePath(Guid blobPath, FileType type, Guid fileId)
            => AdminReleasePath(blobPath, type, fileId.ToString());

        /**
         * The admin file path, for a file of a particular type and name, on a release.
         */
        public static string AdminReleasePath(Guid blobPath, FileType type, string fileName)
            => $"{AdminReleaseDirectoryPath(blobPath, type)}{fileName}";

        /**
         * The admin directory path where files, of a particular type, on a release are stored.
         */
        public static string AdminReleaseDirectoryPath(Guid blobPath, FileType type)
            => $"{blobPath}/{GetUploadFolderForType(type).GetEnumLabel()}/";

        /**
         * The public file path, for a file of a particular type and id, on a release.
         */
        public static string PublicReleasePath(string publicationSlug, string releaseSlug, FileType type,
            Guid fileId)
        {
            return $"{PublicReleaseDirectoryPath(publicationSlug, releaseSlug, type)}{fileId}";
        }

        /**
         * The public file path, for a file of a particular type and name, on a release.
         */
        public static string PublicReleasePath(string publicationSlug, string releaseSlug, FileType type,
            string fileName)
        {
            return $"{PublicReleaseDirectoryPath(publicationSlug, releaseSlug, type)}{fileName}";
        }

        /**
         * The public directory path where files, of a particular type, on a release are stored.
         */
        public static string PublicReleaseDirectoryPath(string publicationSlug, string releaseSlug,
            FileType type)
        {
            return $"{PublicReleaseDirectoryPath(publicationSlug, releaseSlug)}{type.GetEnumLabel()}/";
        }

        /**
         * The top level public directory path where files on a release are stored.
         */
        public static string PublicReleaseDirectoryPath(string publicationSlug, string releaseSlug)
        {
            return $"{publicationSlug}/{releaseSlug}/";
        }

        /**
         * The public file path of the "All files" zip file on a release.
         */
        public static string PublicReleaseAllFilesZipPath(string publicationSlug, string releaseSlug)
        {
            return
                $"{PublicReleaseDirectoryPath(publicationSlug, releaseSlug, Ancillary)}{publicationSlug}_{releaseSlug}.zip";
        }

        private static string AppendPathSeparator(string segment = null)
        {
            return segment == null ? "" : segment + "/";
        }
    }
}
