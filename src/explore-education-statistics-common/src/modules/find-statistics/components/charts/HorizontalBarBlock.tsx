import {
  ChartDataB,
  ChartDefinition,
  createDataForAxis,
  generateReferenceLines,
  getKeysForChart,
  mapNameToNameLabel,
  populateDefaultChartProps,
  StackedBarProps,
} from '@common/modules/find-statistics/components/charts/ChartFunctions';
import React, { Component } from 'react';
import {
  Bar,
  BarChart,
  CartesianGrid,
  LabelList,
  Legend,
  LineChart,
  ResponsiveContainer,
  Tooltip,
  XAxis,
  YAxis,
} from 'recharts';
import LoadingSpinner from '@common/components/LoadingSpinner';

export default class HorizontalBarBlock extends Component<StackedBarProps> {
  public static definition: ChartDefinition = {
    type: 'horizontalbar',
    name: 'Horizontal Bar',

    capabilities: {
      dataSymbols: false,
      stackable: true,
    },

    data: [
      {
        type: 'bar',
        title: 'Bar',
        entryCount: 1,
        targetAxis: 'yaxis',
      },
    ],

    axes: [
      {
        id: 'major',
        title: 'Y Axis',
        type: 'major',
        defaultDataType: 'timePeriods',
      },
      {
        id: 'minor',
        title: 'X Axis',
        type: 'minor',
      },
    ],
  };

  public render() {
    const {
      data,
      meta,
      height,
      width,
      referenceLines,
      stacked = false,
      labels,
      axes,
      legend,
      legendHeight,
    } = this.props;

    if (!axes.major || !data) return <LoadingSpinner />;

    const chartData: ChartDataB[] = createDataForAxis(
      axes.major,
      data.result,
      meta,
    ).map(mapNameToNameLabel(labels, meta.timePeriods));

    const keysForChart = getKeysForChart(chartData);

    return (
      <ResponsiveContainer width={width || '100%'} height={height || 600}>
        <BarChart
          data={chartData}
          layout="vertical"
          margin={{
            left: 30,
            top: legend === 'top' ? 10 : 0,
          }}
        >
          <CartesianGrid
            strokeDasharray="3 3"
            horizontal={axes.minor && axes.minor.showGrid !== false}
            vertical={axes.major && axes.major.showGrid !== false}
          />

          {axes.minor && (
            <XAxis
              type="number"
              hide={axes.minor.visible === false}
              label={{
                angle: -90,
                offset: 0,
                position: 'left',
                value: '',
              }}
              scale="auto"
              height={legend === 'bottom' ? 50 : undefined}
              padding={{ left: 20, right: 20 }}
              tickMargin={10}
            />
          )}

          {axes.major && (
            <YAxis
              type="category"
              dataKey="name"
              hide={axes.major.visible === false}
              label={{
                offset: 5,
                position: 'bottom',
                value: '',
              }}
              scale="auto"
              interval={
                axes.minor && axes.minor.visible !== false
                  ? 'preserveStartEnd'
                  : undefined
              }
            />
          )}

          <Tooltip cursor={false} />
          {(legend === 'top' || legend === 'bottom') && (
            <Legend verticalAlign={legend} height={+(legendHeight || '50')} />
          )}

          {Array.from(keysForChart).map(name => (
            <Bar
              key={name}
              {...populateDefaultChartProps(name, labels[name])}
              stackId={stacked ? 'a' : undefined}
            />
          ))}

          {referenceLines && generateReferenceLines(referenceLines)}
        </BarChart>
      </ResponsiveContainer>
    );
  }
}
