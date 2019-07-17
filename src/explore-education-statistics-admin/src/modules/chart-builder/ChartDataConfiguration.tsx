import {
  ChartSymbol,
  DataSetConfiguration,
} from '@common/services/publicationService';
import * as React from 'react';
import {
  FormFieldset,
  FormSelect,
  FormTextInput,
} from '@common/components/form';
import {
  ChartCapabilities,
  colours,
  symbols,
} from '@common/modules/find-statistics/components/charts/ChartFunctions';

import { SelectOption } from '@common/components/form/FormSelect';

interface Props {
  configuration: DataSetConfiguration;
  capabilities: ChartCapabilities;

  onConfigurationChange?: (value: DataSetConfiguration) => void;
}

const colourOptions: SelectOption[] = colours.map(color => {
  return {
    label: color,
    value: color,
    style: { backgroundColor: `${color}` },
  };
});

const symbolOptions: SelectOption[] = [
  {
    label: 'none',
    value: '',
  },
  ...symbols.map<SelectOption>(symbol => ({
    label: symbol,
    value: symbol,
  })),
];

const ChartDataConfiguration = ({
  configuration,
  capabilities,
  onConfigurationChange,
}: Props) => {
  const [config, setConfig] = React.useState<DataSetConfiguration>(
    configuration,
  );

  const updateConfig = (newConfig: DataSetConfiguration) => {
    setConfig(newConfig);
    if (onConfigurationChange) onConfigurationChange(newConfig);
  };

  return (
    <FormFieldset id={configuration.value} legend="" legendHidden>
      <p>{configuration.name}</p>

      <FormTextInput
        id="label"
        name="label"
        value={config.label}
        label="Enter Label"
        onChange={e =>
          updateConfig({
            ...config,
            label: e.target.value,
          })
        }
      />

      <FormSelect
        id="colour"
        name="colour"
        label="Select colour"
        value={configuration.colour}
        onChange={e =>
          updateConfig({
            ...config,
            colour: e.target.value,
          })
        }
        options={colourOptions}
      />

      {capabilities.dataSymbols && (
        <FormSelect
          id="symbol"
          name="symbol"
          label="Select symbol"
          value={configuration.symbol}
          onChange={e =>
            updateConfig({
              ...config,
              symbol: e.target.value as ChartSymbol,
            })
          }
          options={symbolOptions}
        />
      )}
    </FormFieldset>
  );
};

export default ChartDataConfiguration;
