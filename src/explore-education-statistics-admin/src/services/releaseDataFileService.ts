import { DeleteDataBlockPlan } from '@admin/services/dataBlockService';
import { DataFilePermissions } from '@admin/services/permissionService';
import client from '@admin/services/utils/service';
import { FileInfo } from '@common/services/types/file';
import downloadFile from './utils/file/downloadFile';

interface DataFileInfo extends FileInfo {
  type: 'Data';
  metaFileId: string;
  metaFileName: string;
  rows: number;
  userName: string;
  created: string;
  status: ImportStatusCode;
  replacedBy?: string;
  permissions: DataFilePermissions;
}

export interface DeleteDataFilePlan {
  deleteDataBlockPlan: DeleteDataBlockPlan;
  footnoteIds: string[];
}

export interface DataFile {
  id: string;
  title: string;
  fileName: string;
  fileSize: {
    size: number;
    unit: string;
  };
  rows: number;
  metaFileId: string;
  metaFileName: string;
  userName: string;
  status: ImportStatusCode;
  replacedBy?: string;
  created?: string;
  isDeleting?: boolean;
  isCancelling?: boolean;
  permissions: DataFilePermissions;
}

export type UploadDataFilesRequest =
  | {
      name: string;
      dataFile: File;
      metadataFile: File;
    }
  | {
      replacingFileId: string;
      dataFile: File;
      metadataFile: File;
    };

export type UploadZipDataFileRequest =
  | {
      name: string;
      zipFile: File;
    }
  | {
      replacingFileId: string;
      zipFile: File;
    };

export type ImportStatusCode =
  | 'COMPLETE'
  | 'QUEUED'
  | 'UPLOADING'
  | 'PROCESSING_ARCHIVE_FILE'
  | 'STAGE_1'
  | 'STAGE_2'
  | 'STAGE_3'
  | 'STAGE_4'
  | 'NOT_FOUND'
  | 'FAILED'
  | 'CANCELLING'
  | 'CANCELLED';

export interface DataFileImportStatus {
  status: ImportStatusCode;
  percentageComplete: number;
  stagePercentageComplete: number;
  errors?: string[];
  numberOfRows: number;
}
function mapFile(file: DataFileInfo): DataFile {
  const [size, unit] = file.size.split(' ');

  return {
    ...file,
    title: file.name,
    rows: file.rows || 0,
    fileSize: {
      size: parseInt(size, 10),
      unit,
    },
  };
}

const releaseDataFileService = {
  getDataFiles(releaseId: string): Promise<DataFile[]> {
    return client
      .get<DataFileInfo[]>(`/release/${releaseId}/data`)
      .then(response => {
        const dataFiles = response.filter(file => file.metaFileName.length > 0);
        return dataFiles.map(mapFile);
      });
  },
  getDataFile(releaseId: string, fileId: string): Promise<DataFile> {
    return client
      .get<DataFileInfo>(`/release/${releaseId}/data/${fileId}`)
      .then(mapFile);
  },
  async uploadDataFiles(
    releaseId: string,
    request: UploadDataFilesRequest,
  ): Promise<DataFile> {
    const { dataFile, metadataFile, ...params } = request;

    const data = new FormData();
    data.append('file', dataFile);
    data.append('metaFile', metadataFile);

    const file = await client.post<DataFileInfo>(
      `/release/${releaseId}/data`,
      data,
      {
        params,
      },
    );

    return mapFile(file);
  },
  async uploadZipDataFile(
    releaseId: string,
    request: UploadZipDataFileRequest,
  ): Promise<DataFile> {
    const { zipFile, ...params } = request;

    const data = new FormData();
    data.append('zipFile', zipFile);

    const file = await client.post<DataFileInfo>(
      `/release/${releaseId}/zip-data`,
      data,
      {
        params,
      },
    );

    return mapFile(file);
  },
  getDataFileImportStatus(
    releaseId: string,
    dataFile: DataFile,
  ): Promise<DataFileImportStatus> {
    return client.get<DataFileImportStatus>(
      `/release/${releaseId}/data/${dataFile.id}/import/status`,
    );
  },

  getDeleteDataFilePlan(
    releaseId: string,
    dataFile: DataFile,
  ): Promise<DeleteDataFilePlan> {
    return client.get<DeleteDataFilePlan>(
      `/release/${releaseId}/data/${dataFile.id}/delete-plan`,
    );
  },
  deleteDataFiles(releaseId: string, fileId: string): Promise<void> {
    return client.delete<void>(`/release/${releaseId}/data/${fileId}`);
  },
  downloadFile(releaseId: string, id: string, fileName: string): Promise<void> {
    return client
      .get<Blob>(`/release/${releaseId}/file/${id}`, {
        responseType: 'blob',
      })
      .then(response => downloadFile(response, fileName));
  },
  cancelImport(releaseId: string, fileId: string): Promise<void> {
    return client.post(`/release/${releaseId}/data/${fileId}/import/cancel`);
  },
};

export default releaseDataFileService;
