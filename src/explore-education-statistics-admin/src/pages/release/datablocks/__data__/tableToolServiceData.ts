import {
  SubjectMeta,
  TableDataResponse,
} from '@common/services/tableBuilderService';

export const testSubjectMeta: SubjectMeta = {
  filters: {
    Characteristic: {
      totalValue: '',
      hint: 'Filter by pupil characteristic',
      legend: 'Characteristic',
      name: 'characteristic',
      options: {
        Gender: {
          label: 'Gender',
          options: [
            {
              label: 'Gender female',
              value: 'gender-female',
            },
          ],
        },
      },
    },
  },
  indicators: {
    AbsenceFields: {
      label: 'Absence fields',
      options: [
        {
          value: 'authorised-absence-sessions',
          label: 'Number of authorised absence sessions',
          unit: '',
          name: 'sess_authorised',
          decimalPlaces: 2,
        },
      ],
    },
  },
  locations: {
    localAuthority: {
      legend: 'Local authority',
      options: [{ value: 'barnet', label: 'Barnet' }],
    },
  },
  timePeriod: {
    legend: 'Time period',
    options: [{ label: '2020/21', code: 'AY', year: 2020 }],
  },
};

export const testTableData: TableDataResponse = {
  subjectMeta: {
    publicationName: '',
    boundaryLevels: [],
    footnotes: [],
    subjectName: 'Subject 1',
    geoJsonAvailable: false,
    locations: [
      {
        level: 'localAuthority',
        label: 'Barnet',
        value: 'barnet',
      },
    ],
    timePeriodRange: [{ code: 'AY', year: 2020, label: '2020/21' }],
    indicators: [
      {
        value: 'authorised-absence-sessions',
        label: 'Number of authorised absence sessions',
        unit: '',
        name: 'sess_authorised',
        decimalPlaces: 2,
      },
    ],
    filters: {
      Characteristic: {
        totalValue: '',
        hint: 'Filter by pupil characteristic',
        legend: 'Characteristic',
        name: 'characteristic',
        options: {
          Gender: {
            label: 'Gender',
            options: [
              {
                label: 'Gender female',
                value: 'gender-female',
              },
            ],
          },
        },
      },
    },
  },
  results: [
    {
      timePeriod: '2020_AY',
      measures: {
        'authorised-absence-sessions': '123',
      },
      location: {
        localAuthority: {
          name: 'Barnet',
          code: 'barnet',
        },
      },
      geographicLevel: 'localAuthority',
      filters: ['gender-female'],
    },
  ],
};
