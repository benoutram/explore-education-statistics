import commaList from '@common/lib/utils/string/commaList';
import {
  LocationFilter,
  TimePeriodFilter,
} from '@common/modules/full-table/types/filters';
import React, { useState } from 'react';
import { FullTableMeta } from '@common/modules/full-table/types/fullTable';
import ButtonText from '@common/components/ButtonText';

export function generateTableTitle({
  timePeriodRange,
  locations,
  subjectName,
  publicationName,
}: {
  timePeriodRange: FullTableMeta['timePeriodRange'];
  locations: FullTableMeta['locations'];
  subjectName: FullTableMeta['subjectName'];
  publicationName: FullTableMeta['publicationName'];
}) {
  let timePeriodString = '';

  if (timePeriodRange.length > 0) {
    const startLabel = timePeriodRange[0].label;
    const endLabel = timePeriodRange[timePeriodRange.length - 1].label;

    timePeriodString =
      startLabel === endLabel
        ? ` for ${startLabel}`
        : ` between ${startLabel} and ${endLabel}`;
  }

  const locationsString = ` in ${commaList(
    locations.map(location => location.label),
  )}`;

  return `Table showing '${subjectName}' from '${publicationName}'${locationsString}${timePeriodString}`;
}

interface Props {
  id: string;
  timePeriodRange: TimePeriodFilter[];
  locations: LocationFilter[];
  subjectName: string;
  publicationName: string;
}

const DataTableCaption = ({
  id = 'dataTableCaption',
  timePeriodRange,
  locations,
  subjectName,
  publicationName,
}: Props) => {
  const caption = generateTableTitle({
    timePeriodRange,
    locations,
    subjectName,
    publicationName,
  });

  const [expanded, setExpanded] = useState<boolean>(false);
  const captionLimit = 300;
  return (
    <>
      <strong id={id}>
        {caption.length > captionLimit && !expanded
          ? `${caption.substring(0, captionLimit)}...`
          : caption}{' '}
      </strong>
      {caption.length > captionLimit && (
        <ButtonText
          onClick={() => {
            return expanded ? setExpanded(false) : setExpanded(true);
          }}
        >
          {expanded ? 'View less' : 'View more'}
        </ButtonText>
      )}

      <ul>
        <li>
          <strong>n/a</strong> - value matching this criteria could not be
          found.
        </li>
        <li>
          <strong>x</strong> - value matching this criteria is suppressed.
        </li>
      </ul>
    </>
  );
};

export default DataTableCaption;
