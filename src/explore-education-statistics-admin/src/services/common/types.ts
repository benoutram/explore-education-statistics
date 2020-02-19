import { ExternalMethodology } from '../dashboard/types';

export interface IdTitlePair {
  id: string;
  title: string;
}

export interface IdLabelPair {
  id: string;
  label: string;
}

export interface ValueLabelPair {
  value: string;
  label: string;
}

export interface ContactDetails {
  id: string;
  contactName: string;
  contactTelNo: string;
  teamEmail: string;
  teamName?: string;
}

export interface UserDetails {
  id: string;
  name: string;
}

export interface PrereleaseContactDetails {
  email: string;
  invited: boolean;
}

export interface TimePeriodCoverageGroup {
  category: {
    label: string;
  };
  timeIdentifiers: {
    identifier: ValueLabelPair;
  }[];
}

export interface BasicMethodology {
  id: string;
  title: string;
  status: 'Live' | 'Draft' | 'Approved' | string;
}
export interface BasicPublicationDetails {
  id: string;
  title: string;
  contact?: ContactDetails;
  methodology?: BasicMethodology;
  externalMethodology?: ExternalMethodology;
  themeId: string;
}
