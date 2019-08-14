import { dataApi } from '@common/services/api';
import { Dictionary } from '@common/types';
import { TableData } from '@common/services/tableBuilderService';
import TimePeriod from '@common/services/types/TimePeriod';
import {
  CategoryFilter,
  Indicator,
  LocationFilter,
} from '@frontend/modules/table-tool/components/types/filters';
import { TableHeadersFormValues } from '@frontend/modules/table-tool/components/TableHeadersForm';

export interface SubjectMeta {
  indicators: Indicator[];
  filters: Dictionary<CategoryFilter[]>;
  publicationName: string;
  subjectName: string;
  locations: LocationFilter[];

  footnotes: {
    id: number;
    label: string;
  }[];
  timePeriodRange: TimePeriod[];
}

export interface FullTable /* ™ */ {
  title: string;
  subjectMeta: SubjectMeta;
  results: TableData['results'];
}

export interface Permalink {
  id: string;
  title: string;
  created: string;
  fullTable: FullTable;
  configuration: {
    tableHeadersConfig?: TableHeadersFormValues;
  };
}

export default {
  getPermalink(publicationSlug: string): Promise<Permalink> {
    return dataApi.get(`Permalink/${publicationSlug}`);
  },
};
