import {
  calculateDataRange,
  ChartDataB,
  ChartDefinition,
  conditionallyAdd,
  createDataForAxis,
  getKeysForChart,
  mapNameToNameLabel,
  populateDefaultChartProps,
  StackedBarProps,
} from '@common/modules/find-statistics/components/charts/ChartFunctions';
import React, { Component } from 'react';
import {
  AxisDomain,
  Bar,
  BarChart,
  CartesianGrid,
  Legend,
  ReferenceLine,
  ResponsiveContainer,
  Tooltip,
  XAxis,
  YAxis,
} from 'recharts';

import classnames from 'classnames';

import './charts.scss';

export default class HorizontalBarBlock extends Component<StackedBarProps> {
  public static definition: ChartDefinition = {
    type: 'horizontalbar',
    name: 'Horizontal bar',

    capabilities: {
      dataSymbols: false,
      stackable: true,
      lineStyle: false,
      gridLines: true,
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
      stacked = false,
      labels,
      axes,
      legend,
      legendHeight,
    } = this.props;

    if (
      axes === undefined ||
      axes.major === undefined ||
      data === undefined ||
      meta === undefined
    )
      return <div>Unable to render chart</div>;

    const chartData: ChartDataB[] = createDataForAxis(
      axes.major,
      data.result,
      meta,
    ).map(mapNameToNameLabel(labels, meta.timePeriods));

    const keysForChart = getKeysForChart(chartData);

    const { min, max } = calculateDataRange(chartData);
    const minorAxisDomain: [AxisDomain, AxisDomain] = [
      axes.minor.min || min,
      axes.minor.max || max,
    ];

    return (
      <ResponsiveContainer width={width || '100%'} height={height || 600}>
        <BarChart
          data={chartData}
          layout="vertical"
          className={classnames({ 'legend-bottom': legend === 'bottom' })}
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
              domain={minorAxisDomain}
              height={conditionallyAdd(
                axes.minor && axes.minor.size,
                legend === 'bottom' ? 50 : undefined,
              )}
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
              width={conditionallyAdd(axes.major && axes.major.size)}
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

          {axes.major &&
            axes.major.referenceLines &&
            axes.major.referenceLines.map(referenceLine => (
              <ReferenceLine
                key={`${referenceLine.position}_${referenceLine.label}`}
                y={referenceLine.position}
                label={referenceLine.label}
              />
            ))}

          {axes.minor &&
            axes.minor.referenceLines &&
            axes.minor.referenceLines.map(referenceLine => (
              <ReferenceLine
                key={`${referenceLine.position}_${referenceLine.label}`}
                x={referenceLine.position}
                label={referenceLine.label}
              />
            ))}
        </BarChart>
      </ResponsiveContainer>
    );
  }
}
