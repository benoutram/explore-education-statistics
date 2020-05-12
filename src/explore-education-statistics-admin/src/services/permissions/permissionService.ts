import { GlobalPermissions, User } from '@admin/services/sign-in/types';
import client from '@admin/services/util/service';

export type PreReleaseAccess = 'Before' | 'After' | 'Within' | 'NoneSet';

export interface PreReleaseWindowStatus {
  access: PreReleaseAccess;
  start: Date;
  end: Date;
}

const permissionService = {
  getGlobalPermissions: (): Promise<GlobalPermissions> => {
    return client.get(`/permissions/access`);
  },
  canAccessPrereleasePages: (user?: User): Promise<boolean> => {
    return Promise.resolve(
      user ? user.permissions.canAccessPrereleasePages : false,
    );
  },
  canUpdateRelease: (releaseId: string): Promise<boolean> => {
    return client.get(`/permissions/release/${releaseId}/update`);
  },
  // BAU-324 - temporary stopgap to stop the updating of Release Amendment data files until Phase 2 of
  // Release Versioning is tackled
  canUpdateReleaseDataFiles: (releaseId: string): Promise<boolean> => {
    return client.get(`/permissions/release/${releaseId}/update-data-files`);
  },
  canMarkReleaseAsDraft: (releaseId: string): Promise<boolean> => {
    return client.get(`/permissions/release/${releaseId}/status/draft`);
  },
  canSubmitReleaseForHigherLevelReview: (
    releaseId: string,
  ): Promise<boolean> => {
    return client.get(`/permissions/release/${releaseId}/status/submit`);
  },
  canApproveRelease: (releaseId: string): Promise<boolean> => {
    return client.get(`/permissions/release/${releaseId}/status/approve`);
  },
  canMakeAmendmentOfRelease: (releaseId: string): Promise<boolean> => {
    return client.get(`/permissions/release/${releaseId}/amend`);
  },
  canCreatePublicationForTopic: (topicId: string): Promise<boolean> => {
    return client.get(`/permissions/topic/${topicId}/publication/create`);
  },
  canCreateReleaseForPublication: (publicationId: string): Promise<boolean> => {
    return client.get(
      `/permissions/publication/${publicationId}/release/create`,
    );
  },
  canUpdateMethodology: (methodologyId: string): Promise<boolean> => {
    return client.get(`/permissions/methodology/${methodologyId}/update`);
  },
  canMarkMethodologyAsDraft: (methodologyId: string): Promise<boolean> => {
    return client.get(`/permissions/methodology/${methodologyId}/status/draft`);
  },
  canApproveMethodology: (methodologyId: string): Promise<boolean> => {
    return client.get(
      `/permissions/methodology/${methodologyId}/status/approve`,
    );
  },
  getPreReleaseWindowStatus: (
    releaseId: string,
  ): Promise<PreReleaseWindowStatus> => {
    return client
      .get<{
        preReleaseAccess: PreReleaseAccess;
        preReleaseWindowStartTime: string;
        preReleaseWindowEndTime: string;
      }>(`/permissions/release/${releaseId}/prerelease/status`)
      .then(status => ({
        access: status.preReleaseAccess,
        start: new Date(status.preReleaseWindowStartTime),
        end: new Date(status.preReleaseWindowEndTime),
      }));
  },
};

export default permissionService;
