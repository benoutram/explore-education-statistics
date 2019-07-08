import {
  Axis,
  ChartDataSet,
  ChartType,
  ChartConfiguration,
  ReferenceLine,
  AxisConfigurationItem,
  ChartSymbol,
} from '@common/services/publicationService';
import React, { ReactNode } from 'react';
import {
  Label,
  Line,
  PositionType,
  ReferenceLine as RechartsReferenceLine,
  XAxis,
  XAxisProps,
  YAxis,
  YAxisProps,
} from 'recharts';
import {
  DataBlockData,
  DataBlockMetadata,
  Result,
} from '@common/services/dataBlockService';
import difference from 'lodash/difference';
import { Dictionary } from '@common/types';

export const colours: string[] = [
  '#4763a5',
  '#f5a450',
  '#005ea5',
  '#800080',
  '#C0C0C0',
];

export const symbols: ChartSymbol[] = [
  'circle',
  'square',
  'triangle',
  'cross',
  'star',
];

export function parseCondensedTimePeriodRange(
  range: string,
  separator: string = '/',
) {
  return [range.substring(0, 4), range.substring(4, 6)].join(separator);
}

export interface ChartProps {
  data: DataBlockData;
  meta: DataBlockMetadata;
  labels: Dictionary<ChartConfiguration>;
  axes: Dictionary<AxisConfigurationItem>;
  height?: number;
  width?: number;
  referenceLines?: ReferenceLine[];
}

export interface StackedBarProps extends ChartProps {
  stacked?: boolean;
}

export interface ChartDataOld {
  name: string;
  indicator: string | undefined;
  data?: ChartDataOld[];
  value?: string;
}

export interface DataSetResult {
  dataSet: ChartDataSet;

  results: Result[];
}

export interface ChartDefinition {
  type: ChartType;
  name: string;

  data: {
    type: string;
    title: string;
    entryCount: number | 'multiple';
    targetAxis: string;
  }[];

  axes: {
    id: string;
    title: string;
    type: 'major' | 'minor';
    defaultDataType?: 'timePeriod' | 'location' | 'filters' | 'indicator';
  }[];
}

function calculateAxis(
  axis: Axis,
  position: PositionType,
  angle: number = 0,
  titleSize: number = 25,
) {
  let size = axis.size || 25;
  let title: ReactNode | '';

  if (axis.title) {
    size += titleSize;
    title = (
      <Label position={position} angle={angle}>
        {axis.title}
      </Label>
    );
  }

  return { size, title };
}

export function calculateMargins(
  xAxis: Axis,
  yAxis: Axis,
  referenceLines?: ReferenceLine[],
) {
  const margin = {
    top: 15,
    right: 30,
    left: 60,
    bottom: 25,
  };

  if (referenceLines && referenceLines.length > 0) {
    referenceLines.forEach(line => {
      if (line.x) margin.top = 25;
      if (line.y) margin.left = 75;
    });
  }

  if (xAxis.title) {
    margin.bottom += 25;
  }

  return margin;
}

export function calculateXAxis(xAxis: Axis, axisProps: XAxisProps): ReactNode {
  const { size: height, title } = calculateAxis(xAxis, 'insideBottom');
  return (
    <XAxis {...axisProps} height={height}>
      {title}
    </XAxis>
  );
}

export function calculateYAxis(yAxis: Axis, axisProps: YAxisProps): ReactNode {
  const { size: width, title } = calculateAxis(yAxis, 'left', 270, 90);
  return (
    <YAxis {...axisProps} width={width}>
      {title}
    </YAxis>
  );
}

export function generateReferenceLines(
  referenceLines: ReferenceLine[],
): ReactNode {
  const generateReferenceLine = (line: ReferenceLine, idx: number) => {
    const referenceLineProps = {
      key: `ref_${idx}`,
      ...line,
      stroke: 'black',
      strokeWidth: '2px',

      label: {
        position: 'top',
        value: line.label,
      },
    };

    // Using <Label> in the label property is causing an infinite loop
    // forcing the use of the properties directly as per https://github.com/recharts/recharts/issues/730
    // appears to be a fix, but this is not valid for the types.
    // issue raised https://github.com/recharts/recharts/issues/1710
    // @ts-ignore
    return <RechartsReferenceLine {...referenceLineProps} />;
  };

  return referenceLines.map(generateReferenceLine);
}

export function filterResultsBySingleDataSet(
  dataSet: ChartDataSet,
  results: Result[],
) {
  return results.filter(
    r =>
      dataSet.indicator &&
      Object.keys(r.measures).includes(dataSet.indicator) &&
      (dataSet.filters && difference(r.filters, dataSet.filters).length === 0),
  );
}

function filterResultsForDataSet(ds: ChartDataSet) {
  return (result: Result) => {
    // fail fast with the two things that are most likely to not match
    if (ds.indicator && !Object.keys(result.measures).includes(ds.indicator))
      return false;

    if (ds.filters) {
      if (difference(ds.filters, result.filters).length !== 0) return false;
    }

    if (ds.location) {
      const { location } = result;
      if (
        location.country &&
        ds.location.country &&
        location.country.code !== ds.location.country.code
      )
        return false;
      if (
        location.region &&
        ds.location.region &&
        location.region.code !== ds.location.region.code
      )
        return false;
      if (
        location.localAuthorityDistrict &&
        ds.location.localAuthorityDistrict &&
        location.localAuthorityDistrict.code !==
          ds.location.localAuthorityDistrict.code
      )
        return false;
      if (
        location.localAuthority &&
        ds.location.localAuthority &&
        location.localAuthority.code !== ds.location.localAuthority.code
      )
        return false;
    }

    if (ds.timePeriod) {
      if (ds.timePeriod !== `${result.year}_${result.timeIdentifier}`)
        return false;
    }

    return true;
  };
}

export interface ChartDataB {
  name: string;

  [key: string]: string;
}

export function generateKeyFromDataSet(
  dataSet: ChartDataSet,
  ignoringFields: string[] = [],
) {
  const { indicator, filters, location, timePeriod } = dataSet;
  return [
    indicator,
    ...(filters || []),
    location && location.country && location.country.code,
    location && location.region && location.region.code,
    location &&
      location.localAuthorityDistrict &&
      location.localAuthorityDistrict.code,
    location && location.localAuthority && location.localAuthority.code,
    (!ignoringFields.includes('timePeriod') && timePeriod) || '',
  ].join('_');
}

function generateNameForAxisConfiguration(groupBy: string[], result: Result) {
  return groupBy
    .map(identifier => {
      switch (identifier) {
        case 'timePeriod':
          return `${result.year}_${result.timeIdentifier}`;
        default:
          return '';
      }
    })
    .join('_');
}

function getChartDataForAxis(
  dataForAxis: Result[],
  dataSet: ChartDataSet,
  groupBy: string[],
) {
  return dataForAxis.reduce<ChartDataB[]>(
    (r: ChartDataB[], result) => [
      ...r,
      {
        name: generateNameForAxisConfiguration(groupBy, result),
        [generateKeyFromDataSet(dataSet, groupBy)]:
          result.measures[dataSet.indicator] || 'NaN',
      },
    ],
    [],
  );
}

function reduceCombineChartData(
  newCombinedData: ChartDataB[],
  { name, ...valueData }: { name: string },
) {
  // find and remove the existing matching (by name) entry from the list of data, or create a new one empty one
  const existingDataIndex = newCombinedData.findIndex(
    axisValue => axisValue.name === name,
  );
  const [existingData] =
    existingDataIndex >= 0
      ? newCombinedData.splice(existingDataIndex, 1)
      : [{ name }];

  // put the new entry into the array with any existing and new values added to it
  return [
    ...newCombinedData,
    {
      ...existingData,
      ...valueData,
    },
  ];
}

export function createDataForAxis(
  axisConfiguration: AxisConfigurationItem,
  results: Result[],
) {
  if (axisConfiguration === undefined || results === undefined) return [];

  return axisConfiguration.dataSets.reduce<ChartDataB[]>(
    (combinedChartData, dataSetForAxisConfiguration) => {
      return getChartDataForAxis(
        results.filter(filterResultsForDataSet(dataSetForAxisConfiguration)),
        dataSetForAxisConfiguration,
        axisConfiguration.groupBy,
      ).reduce(reduceCombineChartData, [...combinedChartData]);
    },
    [],
  );
}

export function getKeysForChart(chartData: ChartDataB[]) {
  return Array.from(
    chartData.reduce((setOfKeys, { name: _, ...values }) => {
      return new Set([...Array.from(setOfKeys), ...Object.keys(values)]);
    }, new Set<string>()),
  );
}

export function mapNameToNameLabel(dataLabels: Dictionary<ChartConfiguration>) {
  return ({ name, ...otherdata }: { name: string }) => ({
    ...otherdata,
    name: (dataLabels[name] && dataLabels[name].label) || name,
  });
}

export function populateDefaultChartProps(
  name: string,
  config: ChartConfiguration,
) {
  return {
    dataKey: name,
    isAnimationActive: false,
    name: (config && config.label) || name,
    stroke: config && config.colour,
    fill: config && config.colour,
    unit: (config && config.unit) || '',
  };
}
