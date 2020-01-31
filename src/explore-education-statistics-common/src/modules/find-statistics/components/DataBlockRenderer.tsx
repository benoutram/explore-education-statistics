import React, { useState, useEffect } from 'react';
import DataBlockService, {
  DataBlock,
  DataBlockResponse,
} from '@common/services/dataBlockService';
import LoadingSpinner from '@common/components/LoadingSpinner';
import TimePeriodDataTableRenderer from './TimePeriodDataTableRenderer';
import ChartRenderer, { ChartRendererProps } from './ChartRenderer';
import { parseMetaData, ChartMetaData } from './charts/ChartFunctions';

interface DataBlockWithOptionalResponse extends DataBlock {
  dataBlockResponse?: DataBlockResponse;
}

interface Props {
  datablock: DataBlockWithOptionalResponse;
  renderType: 'table' | 'chart';
}

/**
 * Component that takes a **datablock** (with optional **datablock**.dataBlockResponse)
 * and a **renderType** to handle rendering of tables/charts from datablock data.
 *
 * The component will handle the gathering of the datablockResponse if it was not provided.
 *
 * Acts as a _"shim"_ component to gather and transform datablock props into properties expected by the data/UI rendering components
 */
const DataBlockRenderer = ({ datablock, renderType }: Props) => {
  const [dataBlockResponse, setDataBlockResponse] = useState<
    DataBlockResponse | undefined
  >(datablock.dataBlockResponse);
  const [error, setError] = useState<string>('');

  useEffect(() => {
    if (!dataBlockResponse) {
      DataBlockService.getDataBlockForSubject(datablock.dataBlockRequest)
        .then(setDataBlockResponse)
        .catch(() => {
          setDataBlockResponse(undefined);
          setError('Unable to get data reponse from the server');
        });
    }
  }, [datablock, dataBlockResponse]);

  if (!dataBlockResponse && !!error) {
    return <>{error}</>;
  }
  if (!dataBlockResponse) {
    return <LoadingSpinner text="Loading data" />;
  }
  if (renderType === 'table')
    return (
      <>
        <TimePeriodDataTableRenderer
          heading={datablock.heading}
          response={dataBlockResponse}
          tableHeaders={
            datablock.tables &&
            datablock.tables[0] &&
            datablock.tables[0].tableHeaders
          }
        />
      </>
    );
  if (renderType === 'chart') {
    // There is a presumption that the configuration from the API is valid.
    // The data coming from the API is required to be optional for the ChartRenderer
    // But the data for the charts is required. The charts have validation that
    // prevent them from attempting to render.
    // @ts-ignore
    const chartRendererProps: ChartRendererProps = {
      data: dataBlockResponse,
      meta: parseMetaData(dataBlockResponse.metaData) as ChartMetaData,
      ...(datablock.charts && datablock.charts[0]),
    };
    return (
      <>
        <ChartRenderer {...chartRendererProps} />
      </>
    );
  }
  return null;
};

export default DataBlockRenderer;