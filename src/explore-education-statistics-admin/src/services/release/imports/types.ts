export type ImportStatusCode =
  | 'COMPLETE'
  | 'RUNNING_PHASE_1'
  | 'RUNNING_PHASE_2'
  | 'NOT_FOUND'
  | 'FAILED';

export interface ImportStatus {
  status: ImportStatusCode;
  percentageComplete?: string;
  errors?: string[];
  numberOfRows: number;
}

export default {};