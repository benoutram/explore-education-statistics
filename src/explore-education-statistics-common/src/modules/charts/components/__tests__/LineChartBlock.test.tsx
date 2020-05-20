import {
  testChartConfiguration,
  testChartTableData,
} from '@common/modules/charts/components/__tests__/__data__/testChartData';
import { expectTicks } from '@common/modules/charts/components/__tests__/testUtils';
import LineChartBlock, {
  LineChartProps,
} from '@common/modules/charts/components/LineChartBlock';
import { FullTableMeta } from '@common/modules/table-tool/types/fullTable';
import mapFullTable from '@common/modules/table-tool/utils/mapFullTable';
import { TableDataResult } from '@common/services/tableBuilderService';
import { render } from '@testing-library/react';
import React from 'react';

jest.mock('recharts/lib/util/LogUtils');

describe('LineChartBlock', () => {
  const fullTable = mapFullTable(testChartTableData);
  const props: LineChartProps = {
    ...testChartConfiguration,
    axes: testChartConfiguration.axes as LineChartProps['axes'],
    meta: fullTable.subjectMeta,
    data: fullTable.results,
  };

  const { axes } = props;

  test('renders basic chart correctly', () => {
    const { container } = render(<LineChartBlock {...props} />);

    // axes
    expect(
      container.querySelector('.recharts-cartesian-axis.xAxis'),
    ).toBeInTheDocument();
    expect(
      container.querySelector('.recharts-cartesian-axis.yAxis'),
    ).toBeInTheDocument();

    // grid & grid lines
    expect(
      container.querySelector('.recharts-cartesian-grid'),
    ).toBeInTheDocument();
    expect(
      container.querySelector('.recharts-cartesian-grid-horizontal'),
    ).toBeInTheDocument();
    expect(
      container.querySelector('.recharts-cartesian-grid-vertical'),
    ).toBeInTheDocument();

    // Legend
    expect(
      container.querySelector('.recharts-default-legend'),
    ).toBeInTheDocument();

    const legendItems = container.querySelectorAll('.recharts-legend-item');
    expect(legendItems[0]).toHaveTextContent('Unauthorised absence rate');
    expect(legendItems[1]).toHaveTextContent('Authorised absence rate');
    expect(legendItems[2]).toHaveTextContent('Overall absence rate');

    // expect there to be lines for all 3 data sets
    expect(container.querySelectorAll('.recharts-line')).toHaveLength(3);
  });

  test('major axis can be hidden', () => {
    const { container } = render(
      <LineChartBlock
        {...props}
        axes={{
          ...axes,
          major: {
            ...axes.major,
            visible: false,
          },
        }}
      />,
    );

    expect(
      container.querySelector('.recharts-cartesian-axis.xAxis'),
    ).not.toBeInTheDocument();
  });

  test('minor axis can be hidden', () => {
    const { container } = render(
      <LineChartBlock
        {...props}
        axes={{
          ...axes,
          minor: {
            ...axes.minor,
            visible: false,
          },
        }}
      />,
    );

    expect(
      container.querySelector('.recharts-cartesian-axis.yAxis'),
    ).not.toBeInTheDocument();
  });

  test('both axes can be hidden', () => {
    const { container } = render(
      <LineChartBlock
        {...props}
        axes={{
          ...axes,
          minor: {
            ...axes.minor,
            visible: false,
          },
          major: {
            ...axes.major,
            visible: false,
          },
        }}
      />,
    );

    expect(
      container.querySelector('.recharts-cartesian-axis.yAxis'),
    ).not.toBeInTheDocument();

    expect(
      container.querySelector('.recharts-cartesian-axis.xAxis'),
    ).not.toBeInTheDocument();
  });

  test('can hide legend', () => {
    const { container } = render(<LineChartBlock {...props} legend="none" />);

    expect(
      container.querySelector('.recharts-default-legend'),
    ).not.toBeInTheDocument();
  });

  test('can set dashed line styles', () => {
    const { container } = render(
      <LineChartBlock
        {...props}
        axes={{
          ...axes,
          major: {
            ...axes.major,
            dataSets: [
              {
                indicator: 'unauthorised-absence-rate',
                filters: ['characteristic-total', 'school-type-total'],
                config: {
                  label: 'Unauthorised absence rate',
                  unit: '%',
                  colour: '#4763a5',
                  symbol: 'circle',
                  lineStyle: 'dashed',
                },
              },
              props.axes.major.dataSets[1],
              props.axes.major.dataSets[2],
            ],
          },
        }}
      />,
    );

    expect(
      container.querySelector('.recharts-line-curve[stroke-dasharray="5 5"]'),
    ).toBeInTheDocument();
  });

  test('can set dotted line styles', () => {
    const { container } = render(
      <LineChartBlock
        {...props}
        axes={{
          ...axes,
          major: {
            ...axes.major,
            dataSets: [
              {
                indicator: 'unauthorised-absence-rate',
                filters: ['characteristic-total', 'school-type-total'],
                config: {
                  label: 'Unauthorised absence rate',
                  unit: '%',
                  colour: '#4763a5',
                  symbol: 'circle',
                  lineStyle: 'dotted',
                },
              },
              props.axes.major.dataSets[1],
              props.axes.major.dataSets[2],
            ],
          },
        }}
      />,
    );

    expect(
      container.querySelector('.recharts-line-curve[stroke-dasharray="2 2"]'),
    ).toBeInTheDocument();
  });

  test('can render major axis reference line', () => {
    const { container } = render(
      <LineChartBlock
        {...props}
        axes={{
          ...props.axes,
          major: {
            ...props.axes.major,
            referenceLines: [
              {
                label: 'hello',
                position: '2014_AY',
              },
            ],
          },
        }}
        legend="none"
      />,
    );

    expect(
      container.querySelector('.recharts-reference-line'),
    ).toHaveTextContent('hello');
  });

  test('can render minor axis reference line', () => {
    const { container } = render(
      <LineChartBlock
        {...props}
        axes={{
          ...props.axes,
          minor: {
            ...props.axes.minor,
            min: -10,
            max: 10,
            referenceLines: [
              {
                label: 'hello',
                position: 0,
              },
            ],
          },
        }}
        legend="none"
      />,
    );

    expect(
      container.querySelector('.recharts-reference-line'),
    ).toHaveTextContent('hello');
  });

  test('dies gracefully with bad data', () => {
    const invalidData = (undefined as unknown) as TableDataResult[];
    const invalidMeta = (undefined as unknown) as FullTableMeta;
    const invalidAxes = (undefined as unknown) as LineChartProps['axes'];

    const { container } = render(
      <LineChartBlock
        title=""
        alt=""
        height={300}
        data={invalidData}
        meta={invalidMeta}
        axes={invalidAxes}
      />,
    );
    expect(container).toHaveTextContent('Unable to render chart');
  });

  test('can change width of chart', () => {
    const propsWithSize = {
      ...props,
      width: 200,
    };

    const { container } = render(<LineChartBlock {...propsWithSize} />);

    const responsiveContainer = container.querySelector(
      '.recharts-responsive-container',
    );

    expect(responsiveContainer).toHaveProperty('style');

    if (responsiveContainer) {
      const div = responsiveContainer as HTMLElement;
      expect(div.style.width).toEqual('200px');
    }
  });

  test('can change height of chart', () => {
    const propsWithSize = {
      ...props,
      height: 200,
    };

    const { container } = render(<LineChartBlock {...propsWithSize} />);

    const responsiveContainer = container.querySelector(
      '.recharts-responsive-container',
    );

    expect(responsiveContainer).toHaveProperty('style');

    if (responsiveContainer) {
      const div = responsiveContainer as HTMLElement;
      expect(div.style.height).toEqual('200px');
    }
  });

  test('can limit range of minor ticks to default', () => {
    const { container } = render(
      <LineChartBlock
        {...props}
        axes={{
          major: props.axes.major,
          minor: {
            ...props.axes.minor,
            tickConfig: 'default',
          },
        }}
      />,
    );

    expectTicks(container, 'y', '0', '2', '4', '6');
  });

  test('can limit range of minor ticks to start and end', () => {
    const { container } = render(
      <LineChartBlock
        {...props}
        axes={{
          major: props.axes.major,
          minor: {
            ...props.axes.minor,
            tickConfig: 'startEnd',
          },
        }}
      />,
    );

    expectTicks(container, 'y', '0', '6');
  });

  test('can limit range of minor ticks to custom', () => {
    const { container } = render(
      <LineChartBlock
        {...props}
        axes={{
          major: props.axes.major,
          minor: {
            ...props.axes.minor,
            tickConfig: 'custom',
            tickSpacing: 1,
          },
        }}
      />,
    );

    expectTicks(container, 'y', '0', '1', '2', '3', '4', '5', '6');
  });

  test('can limit range of major ticks to default', () => {
    const propsWithTicks: LineChartProps = {
      ...props,
      axes: {
        minor: props.axes.minor,
        major: {
          ...props.axes.major,
          tickConfig: 'default',
        },
      },
    };

    const { container } = render(<LineChartBlock {...propsWithTicks} />);

    expectTicks(
      container,
      'x',
      '2012/13',
      '2013/14',
      '2014/15',
      '2015/16',
      '2016/17',
    );
  });

  test('can limit range of major ticks to start and end', () => {
    const { container } = render(
      <LineChartBlock
        {...props}
        axes={{
          minor: props.axes.minor,
          major: {
            ...props.axes.major,
            tickConfig: 'startEnd',
          },
        }}
      />,
    );

    expectTicks(container, 'x', '2012/13', '2016/17');
  });

  test('can limit range of major ticks to custom', () => {
    const propsWithTicks: LineChartProps = {
      ...props,
      axes: {
        minor: props.axes.minor,
        major: {
          ...props.axes.major,
          tickConfig: 'custom',
          tickSpacing: 2,
        },
      },
    };

    const { container } = render(<LineChartBlock {...propsWithTicks} />);

    expectTicks(container, 'x', '2012/13', '2014/15', '2016/17');
  });

  test('can sort by name', () => {
    const { container } = render(
      <LineChartBlock
        {...props}
        axes={{
          minor: props.axes.minor,
          major: {
            ...props.axes.major,
            sortBy: 'name',
            sortAsc: true,
          },
        }}
      />,
    );

    expectTicks(
      container,
      'x',
      '2012/13',
      '2013/14',
      '2014/15',
      '2015/16',
      '2016/17',
    );
  });

  test('can sort by name descending', () => {
    const { container } = render(
      <LineChartBlock
        {...props}
        axes={{
          minor: props.axes.minor,
          major: {
            ...props.axes.major,
            sortBy: 'name',
            sortAsc: false,
          },
        }}
      />,
    );

    expectTicks(
      container,
      'x',
      '2016/17',
      '2015/16',
      '2014/15',
      '2013/14',
      '2012/13',
    );
  });

  test('can filter a data range', () => {
    const { container } = render(
      <LineChartBlock
        {...props}
        axes={{
          minor: props.axes.minor,
          major: {
            ...props.axes.major,
            sortBy: 'name',
            sortAsc: true,
            min: 0,
            max: 1,
          },
        }}
      />,
    );

    expectTicks(container, 'x', '2012/13', '2013/14');
  });
});
