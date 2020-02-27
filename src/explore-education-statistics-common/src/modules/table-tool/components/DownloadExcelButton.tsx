import ButtonText from '@common/components/ButtonText';
import { generateTableTitle } from '@common/modules/table-tool/components/DataTableCaption';
import { FullTableMeta } from '@common/modules/table-tool/types/fullTable';
import { Dictionary } from '@common/types';
import last from 'lodash/last';
import sum from 'lodash/sum';
import React, { RefObject } from 'react';
import { CellObject, utils, WorkSheet, writeFile } from 'xlsx';

interface Props {
  publicationSlug: string;
  subjectMeta: FullTableMeta;
  tableRef: RefObject<HTMLElement>;
}

/**
 * Append a {@param title} to the beginning of the {@param sheet}.
 *
 * Might not be the most efficient way of doing this, as it
 * relies heavily on the `sheet_to_json` util.
 */
export function appendTitle(sheet: WorkSheet, title: string) {
  const existingRows = utils.sheet_to_json<string[]>(sheet, {
    header: 1,
    defval: '',
  });

  // We use the last as it gives us the length more reliably than
  // the first row due to the top-left corner of the table being empty.
  const totalCols = last(existingRows)?.length ?? 0;
  const emptyRow = Array(totalCols).fill('');

  // Top-left corner might not exist in the array for some
  // reason, so make this an empty string manually.
  if (typeof existingRows[0][0] === 'undefined') {
    existingRows[0][0] = '';
  }

  utils.sheet_add_aoa(sheet, [
    [title, ...emptyRow.slice(1)],
    [...emptyRow],
    ...existingRows,
  ]);

  // Preserve merges by shifting them all down by 2 rows
  // eslint-disable-next-line no-param-reassign
  sheet['!merges'] = sheet['!merges']?.map(merge => ({
    s: {
      r: merge.s.r + 2,
      c: merge.s.c,
    },
    e: {
      r: merge.e.r + 2,
      c: merge.e.c,
    },
  }));

  return sheet;
}

/**
 * Explicitly set column widths for a {@param sheet} using
 * the width of the cell with the most content in it.
 * SheetJs doesn't automatically generate our column widths
 * so the spreadsheet would look quite squished without this.
 */
export function appendColumnWidths(sheet: WorkSheet): WorkSheet {
  const columnWidths = Object.entries(sheet)
    .filter(([key]) => !key.startsWith('!'))
    .reduce<Dictionary<number>>((acc, [key, value]: [string, CellObject]) => {
      const [column] = key.split(/[0-9]+/);

      if (typeof acc[column] === 'undefined') {
        acc[column] = 0;
      }

      let length = 0;

      if (typeof value.v === 'string') {
        // eslint-disable-next-line prefer-destructuring
        length = value.v.length;
      } else if (typeof value.v === 'number') {
        // eslint-disable-next-line prefer-destructuring
        length = value.v.toString().length;
      }

      if (length > acc[column]) {
        acc[column] = length;
      }

      return acc;
    }, {});

  // eslint-disable-next-line no-param-reassign
  sheet['!cols'] = Object.entries(columnWidths)
    .map(([key, value]) => {
      // Convert column letters to a single number so they
      // can be sorted in the way that Excel expects
      const keyHash = sum(key.split('').map(char => char.charCodeAt(0)));

      return [keyHash, value];
    })
    .sort(([a], [b]) => a - b)
    .map(([, value]) => {
      return value ? { wch: value } : {};
    });

  return sheet;
}

/**
 * Append {@param footnotes} to the bottom of a {@param sheet}.
 */
export function appendFootnotes(
  sheet: WorkSheet,
  footnotes: FullTableMeta['footnotes'],
): WorkSheet {
  const [, lastCell] = sheet['!ref']?.split(':') ?? [];

  if (!lastCell) {
    return sheet;
  }

  const [lastColumn, lastRow] = lastCell.split(/([0-9]+)/);
  const footnoteStartRow = parseInt(lastRow, 10) + 2;

  footnotes.forEach((footnote, index) => {
    // eslint-disable-next-line no-param-reassign
    sheet[`A${footnoteStartRow + index}`] = {
      t: 's',
      v: `(${index + 1}) ${footnote.label}`,
    } as CellObject;
  });

  // eslint-disable-next-line no-param-reassign
  sheet['!ref'] = `A1:${lastColumn}${footnoteStartRow + footnotes.length - 1}`;

  return sheet;
}

const DownloadExcelButton = ({
  publicationSlug,
  subjectMeta,
  tableRef,
}: Props) => {
  const { footnotes } = subjectMeta;

  // Try to find table element within the ref
  let tableEl: HTMLTableElement | null = null;

  if (tableRef.current) {
    if (tableRef.current.tagName.toLowerCase() === 'table') {
      tableEl = tableRef.current as HTMLTableElement;
    } else {
      tableEl = tableRef.current.querySelector('table');
    }

    // Don't render anything if no table could be found
    if (!tableEl) {
      return null;
    }
  }

  return (
    <ButtonText
      onClick={() => {
        const workBook = utils.table_to_book(tableEl, {
          raw: true,
        });
        const sheet = workBook.Sheets[workBook.SheetNames[0]];

        appendColumnWidths(sheet);
        appendTitle(sheet, generateTableTitle(subjectMeta));
        appendFootnotes(sheet, footnotes);

        writeFile(workBook, `data-${publicationSlug}.xlsx`, {
          type: 'binary',
        });
      }}
    >
      Download table as Excel spreadsheet (XLSX)
    </ButtonText>
  );
};

export default DownloadExcelButton;
