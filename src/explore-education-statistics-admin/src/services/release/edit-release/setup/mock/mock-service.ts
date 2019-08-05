import {UpdateReleaseSetupDetailsRequest} from "@admin/services/release/edit-release/setup/types";
import {getCaptureGroups,} from '@admin/services/util/mock/mock-service';
import MockAdapter from 'axios-mock-adapter';

export default async (mock: MockAdapter) => {
  const mockData = (await import(
    /* webpackChunkName: "mock-data" */ './mock-data'
  )).default;

  const mockReferenceData = (await import(
    /* webpackChunkName: "mock-dashboard-data" */ '@admin/pages/DummyReferenceData'
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

    const timePeriodCoverage = mockReferenceData.findTimePeriodCoverageOption(
      updateRequest.timePeriodCoverage.value,
    );

      /* eslint-disable no-param-reassign */
    existingRelease.timePeriodCoverageCode = updateRequest.timePeriodCoverage.value;
    existingRelease.scheduledPublishDate = updateRequest.publishScheduled;
    existingRelease.nextReleaseExpectedDate =
      updateRequest.nextReleaseExpected;
    existingRelease.releaseType = mockReferenceData.findReleaseType(updateRequest.releaseTypeId);
    /* eslint-enable no-param-reassign */

    return [200];
  });
};
