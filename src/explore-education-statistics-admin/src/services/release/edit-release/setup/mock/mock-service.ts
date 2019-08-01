import {UpdateReleaseSetupDetailsRequest} from "@admin/services/release/edit-release/setup/types";
import {getCaptureGroups,} from '@admin/services/util/mock/mock-service';
import MockAdapter from 'axios-mock-adapter';

export default async (mock: MockAdapter) => {
  const mockData = (await import(
    /* webpackChunkName: "mock-data" */ './mock-data'
  )).default;

  const getReleaseSetupDetailsUrl = /\/release\/(.*)\/setup/;
  const updateReleaseSetupDetailsUrl = /\/release\/(.*)\/setup/;

  mock.onGet(getReleaseSetupDetailsUrl).reply(({ url }) => {
    const [releaseId] = getCaptureGroups(getReleaseSetupDetailsUrl, url);
    return [200, mockData.getReleaseSetupDetailsForRelease(releaseId)];
  });

  mock.onPost(updateReleaseSetupDetailsUrl).reply(config => {
    const updateRequest = JSON.parse(
      config.data,
    ) as UpdateReleaseSetupDetailsRequest;

    const existingRelease = mockData.getReleaseSetupDetailsForRelease(
      updateRequest.releaseId,
    );

    /* eslint-disable no-param-reassign */
    existingRelease.timePeriodCoverageCode =
      updateRequest.timePeriodCoverageCode;
    existingRelease.timePeriodCoverageStartYear = updateRequest.timePeriodCoverageStartYear;
    existingRelease.scheduledPublishDate = updateRequest.scheduledPublishDate;
    existingRelease.nextReleaseExpectedDate =
      updateRequest.nextReleaseExpectedDate;
    existingRelease.releaseType = updateRequest.releaseType;
    /* eslint-enable no-param-reassign */

    return [200];
  });
};
