/* eslint-disable */
import Tabs from '@common/components/Tabs';
import TabsSection from '@common/components/TabsSection';
import { MapFeature } from '@common/modules/find-statistics/components/charts/MapBlock';
import SummaryRenderer, {
  SummaryRendererProps,
} from '@common/modules/find-statistics/components/SummaryRenderer';
import TableRenderer2, {
  Props as TableRendererProps,
} from '@common/modules/find-statistics/components/TableRenderer2';
import { baseUrl } from '@common/services/api';
import { Chart, DataQuery, Summary } from '@common/services/publicationService';
import {
  CharacteristicsData,
  PublicationMeta,
} from '@common/services/tableBuilderService';

import React, { Component, ReactNode } from 'react';
import DataBlockService, {
  DataBlockRequest,
  DataBlockMetadata,
  DataBlockData,
} from '@common/services/dataBlockService';
import ChartRenderer, {
  ChartRendererProps,
} from '@common/modules/find-statistics/components/ChartRenderer';

export interface DataBlockProps {
  type: string;
  heading?: string;
  dataBlockRequest?: DataBlockRequest;
  charts?: Chart[];
  summary?: Summary;
  data?: CharacteristicsData;
  meta?: PublicationMeta;
  height?: number;
  showTables?: boolean;
  additionalTabContent?: ReactNode;
}

interface DataBlockState {
  charts?: ChartRendererProps[];
  // downloads?: any[];
  tables?: TableRendererProps[];
  summary?: SummaryRendererProps;
}

class DataBlock extends Component<DataBlockProps, DataBlockState> {
  public static defaultProps = {
    showTables: true,
  };

  public state: DataBlockState = {};

  private currentDataQuery?: DataQuery = undefined;

  public async componentDidMount() {
    const { dataBlockRequest, data, meta } = this.props;

    if (dataBlockRequest) {
      const result = await DataBlockService.getDataBlockForSubject(
        dataBlockRequest,
      );

      this.parseDataResponse(result.data, result.metaData);
    } else {
      // this.parseDataResponse(data, meta);
    }
  }

  public async componentWillUnmount() {
    this.currentDataQuery = undefined;
  }

  private parseDataResponse(
    json: DataBlockData,
    jsonMeta: DataBlockMetadata,
  ): void {
    const newState: DataBlockState = {};

    if (json.result.length > 0) {
      newState.tables = [{ data: json, meta: jsonMeta }];
    }

    const { charts, summary } = this.props;

    if (charts) {
      newState.charts = charts.map(chart => ({
        ...chart,
        geometry: chart.geometry as MapFeature,
        data: json,
        meta: jsonMeta,
      }));
    }

    if (summary) {
      newState.summary = {
        ...summary,
        data: json,
        meta: jsonMeta,
      };
    }
    this.setState(newState);
  }

  public render() {
    const id = new Date().getDate();

    const { heading, height, showTables, additionalTabContent } = this.props;
    const { charts, summary, tables } = this.state;

    return (
      <div className="govuk-datablock" data-testid={`DataBlock ${heading}`}>
        <Tabs>
          {summary && (
            <TabsSection id={`datablock_${id}_summary`} title="Summary">
              <h3>{heading}</h3>
              <SummaryRenderer {...summary} />
            </TabsSection>
          )}

          {tables && showTables && (
            <TabsSection id={`datablock_${id}_tables`} title="Data tables">
              <h3>{heading}</h3>
              {tables.map((table, idx) => {
                const key = `${id}0_table_${idx}`;

                return <TableRenderer2 key={key} {...table} />;
              })}
              {additionalTabContent}
            </TabsSection>
          )}

          {charts && (
            <TabsSection
              id={`datablock_${id}_charts`}
              title="Charts"
              lazy={false}
            >
              <h3>{heading}</h3>
              {charts.map((chart, idx) => {
                const key = `${id}_chart_${idx}`;

                return <ChartRenderer key={key} {...chart} height={height} />;
              })}
              {additionalTabContent}
            </TabsSection>
          )}

          <TabsSection id={`datablock_${id}_downloads`} title="Data downloads">
            <p>
              You can customise and download data as Excel, .csv or .pdf files.
              Our data can also be accessed via an API.
            </p>
            <div className="govuk-inset-text">
              Data downloads have not yet been implemented within the service.
            </div>
            {additionalTabContent}
          </TabsSection>
        </Tabs>
      </div>
    );
  }
}

export default DataBlock;
