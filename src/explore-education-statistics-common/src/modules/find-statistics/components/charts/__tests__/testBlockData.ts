/* eslint-disable @typescript-eslint/camelcase */
import {
  DataBlockData,
  DataBlockMetadata,
  DataBlockResponse,
  GeographicLevel,
} from '@common/services/dataBlockService';
import { Axis } from '@common/services/publicationService';
import { ChartProps } from '@common/modules/find-statistics/components/charts/AbstractChart';

const data: DataBlockData = {
  publicationId: 'test',
  releaseDate: new Date(),
  releaseId: 1,
  subjectId: 1,
  geographicLevel: GeographicLevel.National,
  result: [
    {
      filters: [1],
      location: {
        country: {
          country_code: 'E92000001',
          country_name: 'England',
        },
        region: {
          region_code: '',
          region_name: '',
        },
        localAuthority: {
          new_la_code: '',
          old_la_code: '',
          la_name: '',
        },
        localAuthorityDistrict: {
          sch_lad_code: '',
          sch_lad_name: '',
        },
      },
      measures: {
        '28': '7.5',
        '26': '9.4',
        '23': '1.9',
      },
      timeIdentifier: 'HT6',
      year: 2014,
    },
    {
      filters: [1],
      location: {
        country: {
          country_code: 'E92000001',
          country_name: 'England',
        },
        region: {
          region_code: '',
          region_name: '',
        },
        localAuthority: {
          new_la_code: '',
          old_la_code: '',
          la_name: '',
        },
        localAuthorityDistrict: {
          sch_lad_code: '',
          sch_lad_name: '',
        },
      },
      measures: {
        '28': '7.5',
        '26': '9.4',
        '23': '1.9',
      },
      timeIdentifier: 'HT6',
      year: 2015,
    },
  ],
};

const labels = {
  '28': 'Authorised absence rate',
  '26': 'Overall absence rate',
  '23': 'Unauthorised absence rate',
};

const metaData: DataBlockMetadata = {
  filters: {
    '1': {
      label: 'All Schools',
      value: 1,
    },
  },

  indicators: {
    '28': {
      label: 'Authorised absence rate',
      unit: '%',
      value: 28,
    },
  },

  timePeriods: {
    '2014': {
      label: '2014',
      value: 2014,
    },
    '2015': {
      label: '2015',
      value: 2015,
    },
  },
};

const AbstractChartProps: ChartProps = {
  data,
  labels,
  chartDataKeys: ['23', '26', '28'],
  xAxis: { title: 'test x axis' },
  yAxis: { title: 'test y axis' },
};

export default {
  AbstractChartProps,
  testBlockData: data,
  labels,
};
