import { useReleaseState } from '@admin/pages/release/edit-release/content/ReleaseContext';
import Button from '@common/components/Button';
import Details from '@common/components/Details';
import { FormSelect } from '@common/components/form';
import KeyStatTile from '@common/modules/find-statistics/components/KeyStatTile';
import {
  locationLevelKeys,
  LocationLevelKeys,
  TimePeriodQuery,
} from '@common/modules/table-tool/services/tableBuilderService';
import React, { useMemo, useState } from 'react';
import { DataBlockRequest } from '@common/services/dataBlockService';

interface Props {
  onSelect: (selectedDataBlockId: string) => void;
  onCancel?: () => void;
  hideCancel?: boolean;
  label?: string;
}

const KeyIndicatorSelectForm = ({
  onSelect,
  onCancel = () => {},
  hideCancel = false,
  label = 'Select a key indicator',
}: Props) => {
  const { availableDataBlocks } = useReleaseState();
  const [selectedDataBlockId, setSelectedDataBlockId] = useState('');

  const keyIndicatorDatablocks = useMemo(() => {
    return availableDataBlocks.filter(db => {
      const req = db.dataBlockRequest;
      const timePeriod = req.timePeriod as TimePeriodQuery;
      return (
        req.indicators.length !== 1 ||
        timePeriod.startYear !== timePeriod.endYear ||
        locationLevelKeys.some(key => req[key]?.length !== 1)
        // NOTE(mark): No check for number of filters because they cannot tell us whether
        // there is a single result
      );
    });
  }, [availableDataBlocks]);

  function getKeyStatPreview() {
    const selectedDataBlock = availableDataBlocks.find(
      datablock => datablock.id === selectedDataBlockId,
    );
    return selectedDataBlock ? (
      <section>
        <Details
          className="govuk-!-margin-top-3"
          summary="Key statistic preview"
          open
          onToggle={() => {}}
        >
          <KeyStatTile {...selectedDataBlock} />
        </Details>
      </section>
    ) : null;
  }

  return (
    <>
      <FormSelect
        className="govuk-!-margin-right-1"
        id="id"
        name="key_indicator_select"
        label={label}
        value={selectedDataBlockId}
        onChange={e => setSelectedDataBlockId(e.target.value)}
        options={[
          {
            label: 'Select a data block',
            value: '',
          },
          ...keyIndicatorDatablocks.map(dataBlock => ({
            label: dataBlock.name || '',
            value: dataBlock.id || '',
          })),
        ]}
      />
      {getKeyStatPreview()}
      {selectedDataBlockId !== '' && (
        <Button onClick={() => onSelect(selectedDataBlockId)}>Embed</Button>
      )}
      {!hideCancel && (
        <Button className="govuk-button--secondary" onClick={onCancel}>
          Cancel
        </Button>
      )}
    </>
  );
};

export default KeyIndicatorSelectForm;
