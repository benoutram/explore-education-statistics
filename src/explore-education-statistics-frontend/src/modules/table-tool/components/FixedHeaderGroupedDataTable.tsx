import throttle from 'lodash/throttle';
import React, { forwardRef, Ref, useEffect, useRef } from 'react';
import DataTableKeys from './DataTableKeys';
import styles from './FixedHeaderGroupedDataTable.module.scss';
import MultiHeaderTable, { RowGroup } from './MultiHeaderTable';

const dataTableCaption = 'dataTableCaption';

interface Props {
  caption: string;
  headers: string[][];
  innerRef?: Ref<HTMLElement>;
  rowGroups: RowGroup[];
}

const FixedHeaderGroupedDataTable = forwardRef<HTMLElement, Props>(
  (props, ref) => {
    const { caption } = props;

    const mainTableRef = useRef<HTMLTableElement>(null);
    const headerTableRef = useRef<HTMLTableElement>(null);
    const columnTableRef = useRef<HTMLTableElement>(null);
    const intersectionTableRef = useRef<HTMLTableElement>(null);

    const setStickyElementSizes = throttle(() => {
      if (mainTableRef.current) {
        const mainTableEl = mainTableRef.current;

        const cloneCellHeightWidth = (
          selector: string,
          tableEl: HTMLTableElement,
        ) => {
          const tableCells = tableEl.querySelectorAll<HTMLTableCellElement>(
            selector,
          );

          mainTableEl
            .querySelectorAll<HTMLTableCellElement>(selector)
            .forEach((el, index) => {
              tableCells[index].style.height = `${el.offsetHeight}px`;
              tableCells[index].style.width = `${el.offsetWidth}px`;
            });
        };

        if (headerTableRef.current) {
          headerTableRef.current.style.width = `${mainTableEl.offsetWidth}px`;

          cloneCellHeightWidth('thead th', headerTableRef.current);
        }

        if (columnTableRef.current) {
          cloneCellHeightWidth(
            'thead tr:first-child th:first-child',
            columnTableRef.current,
          );
          cloneCellHeightWidth('tbody th', columnTableRef.current);
        }

        if (intersectionTableRef.current) {
          cloneCellHeightWidth(
            'thead tr:first-child th:first-child',
            intersectionTableRef.current,
          );
        }
      }
    }, 200);

    useEffect(() => {
      setTimeout(() => {
        setStickyElementSizes();

        window.addEventListener('resize', setStickyElementSizes);
      }, 200);

      return () => {
        window.removeEventListener('resize', setStickyElementSizes);
      };
    });

    return (
      <figure className={styles.figure} ref={ref}>
        <figcaption>
          <strong id={dataTableCaption}>{caption}</strong>

          <DataTableKeys />
        </figcaption>

        <div
          className={styles.container}
          role="region"
          tabIndex={-1}
          onScroll={event => {
            const { scrollLeft, scrollTop } = event.currentTarget;

            if (headerTableRef.current) {
              headerTableRef.current.style.top = `${scrollTop}px`;
            }

            if (columnTableRef.current) {
              columnTableRef.current.style.left = `${scrollLeft}px`;
            }

            if (intersectionTableRef.current) {
              intersectionTableRef.current.style.top = `${scrollTop}px`;
              intersectionTableRef.current.style.left = `${scrollLeft}px`;
            }
          }}
        >
          <MultiHeaderTable
            {...props}
            className={styles.intersectionTable}
            ref={intersectionTableRef}
            ariaHidden
            isStickyColumn
            isStickyHeader
          />
          <MultiHeaderTable
            {...props}
            className={styles.columnTable}
            ref={columnTableRef}
            ariaHidden
            isStickyColumn
          />
          <MultiHeaderTable
            {...props}
            className={styles.headerTable}
            ref={headerTableRef}
            ariaHidden
            isStickyHeader
          />

          <MultiHeaderTable ref={mainTableRef} {...props} />
        </div>
      </figure>
    );
  },
);

FixedHeaderGroupedDataTable.displayName = 'FixedHeaderGroupedDataTable';

export default FixedHeaderGroupedDataTable;
