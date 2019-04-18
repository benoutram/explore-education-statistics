import React from 'react';
import {
  Bar,
  BarChart,
  CartesianGrid,
  Legend,
  ResponsiveContainer,
  Tooltip,
  XAxis,
  YAxis,
} from 'recharts';

import { colours } from './Charts';
import {
  AbstractChart,
  ChartProps,
} from '@common/modules/find-statistics/components/charts/AbstractChart';

interface StackedBarHorizontalProps extends ChartProps {
  stacked?: boolean;
}

export class HorizontalBarBlock extends AbstractChart<
  StackedBarHorizontalProps
> {
  public render() {
    const chartData = this.props.characteristicsData.result.map(data => {
      return data.indicators;
    });

    return (
      <ResponsiveContainer width={900} height={this.props.height || 600}>
        <BarChart
          data={chartData}
          layout="vertical"
          margin={this.calculateMargins()}
        >
          <YAxis type="category" dataKey={this.props.yAxis.key || 'name'} />
          <CartesianGrid />
          <XAxis type="number" />
          <Tooltip cursor={false} />
          <Legend />

          {this.props.chartDataKeys.map((key, index) => (
            <Bar
              key={index}
              dataKey={key}
              name={this.props.labels[key]}
              fill={colours[index]}
              stackId={this.props.stacked ? 'a' : undefined}
            />
          ))}

          {this.generateReferenceLines()}
        </BarChart>
      </ResponsiveContainer>
    );
  }
}
