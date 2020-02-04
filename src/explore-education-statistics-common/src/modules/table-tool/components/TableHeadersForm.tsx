import Button from '@common/components/Button';
import Details from '@common/components/Details';
import { FormGroup, Formik } from '@common/components/form';
import reorder from '@common/lib/utils/reorder';
import Yup from '@common/lib/validation/yup';
import { FilterOption } from '@common/modules/full-table/services/tableBuilderService';
import { Filter } from '@common/modules/full-table/types/filters';
import { PickByType } from '@common/types';
import classNames from 'classnames';
import { Form, FormikProps } from 'formik';
import React, { useMemo } from 'react';
import { DragDropContext, Draggable, Droppable } from 'react-beautiful-dnd';
import FormFieldSortableList from './FormFieldSortableList';
import FormFieldSortableListGroup from './FormFieldSortableListGroup';
import styles from './TableHeadersForm.module.scss';

interface Props {
  initialValues?: TableHeadersFormValues;
  onSubmit: (values: TableHeadersFormValues) => void;
}

export interface TableHeadersFormValues {
  columnGroups: Filter[][];
  columns: Filter[];
  rowGroups: Filter[][];
  rows: Filter[];
}

const TableHeadersForm = ({
  onSubmit,
  initialValues = {
    columnGroups: [],
    columns: [],
    rowGroups: [],
    rows: [],
  },
}: Props) => {
  const formInitialValues = useMemo(() => ({ ...initialValues }), [
    initialValues,
  ]);

  return (
    <Details summary="Re-order table headers">
      <p className="govuk-hint">
        Drag and drop the options below to re-order the table headers. For
        keyboard users, select and deselect a draggable item with space and use
        the arrow keys to move a selected item.
      </p>
      <div className="govuk-visually-hidden">
        To move a draggable item, select and deselect the item with space and
        use the arrow keys to move a selected item. If you are using a screen
        reader disable scan mode.
      </div>
      <Formik<TableHeadersFormValues>
        enableReinitialize
        initialValues={formInitialValues}
        validationSchema={Yup.object<TableHeadersFormValues>({
          rowGroups: Yup.array()
            .of(
              Yup.array()
                .of<FilterOption>(Yup.object())
                .ensure(),
            )
            .min(
              formInitialValues.columnGroups.length +
                formInitialValues.rowGroups.length >
                1
                ? 1
                : 0,
              'Must have at least one row group',
            ),
          columnGroups: Yup.array()
            .of(
              Yup.array()
                .of<FilterOption>(Yup.object())
                .ensure(),
            )
            .min(
              formInitialValues.columnGroups.length +
                formInitialValues.rowGroups.length >
                1
                ? 1
                : 0,
              'Must have at least one column group',
            ),
          columns: Yup.array()
            .of<FilterOption>(Yup.object())
            .ensure(),
          rows: Yup.array()
            .of<FilterOption>(Yup.object())
            .ensure(),
        })}
        onSubmit={onSubmit}
        render={(form: FormikProps<TableHeadersFormValues>) => {
          return (
            <Form>
              <FormGroup>
                <div className="govuk-grid-row">
                  <div className="govuk-grid-column-three-quarters-from-desktop">
                    <DragDropContext
                      onDragEnd={result => {
                        if (!result.destination) {
                          return;
                        }

                        const { source, destination } = result;

                        const destinationId = destination.droppableId as
                          | 'columnGroups'
                          | 'rowGroups';

                        const sourceId = source.droppableId as
                          | 'columnGroups'
                          | 'rowGroups';

                        const destinationGroup = form.values[destinationId];

                        if (destinationId === sourceId) {
                          form.setFieldTouched(destinationId);
                          form.setFieldValue(
                            destinationId,
                            reorder(
                              destinationGroup,
                              source.index,
                              destination.index,
                            ),
                          );
                        } else {
                          const sourceClone = Array.from(form.values[sourceId]);
                          const destinationClone = Array.from(destinationGroup);

                          const [sourceItem] = sourceClone.splice(
                            source.index,
                            1,
                          );
                          destinationClone.splice(
                            destination.index,
                            0,
                            sourceItem,
                          );

                          form.setFieldTouched(sourceId);
                          form.setFieldValue(sourceId, sourceClone);

                          form.setFieldTouched(destinationId);
                          form.setFieldValue(destinationId, destinationClone);
                        }
                      }}
                    >
                      <div className={styles.axisContainer}>
                        <FormFieldSortableListGroup<
                          PickByType<TableHeadersFormValues, Filter[][]>
                        >
                          name="rowGroups"
                          legend="Row groups"
                          groupLegend="Row group"
                        />
                      </div>

                      <div className={styles.axisContainer}>
                        <FormFieldSortableListGroup<
                          PickByType<TableHeadersFormValues, Filter[][]>
                        >
                          name="columnGroups"
                          legend="Column groups"
                          groupLegend="Column group"
                        />
                      </div>
                    </DragDropContext>
                  </div>
                  <div className="govuk-grid-column-one-quarter-from-desktop">
                    <DragDropContext
                      onDragEnd={result => {
                        if (!result.destination) {
                          return;
                        }

                        const { source, destination } = result;

                        if (source.index === destination.index) {
                          return;
                        }

                        const columns = [...form.values.columns];
                        const rows = [...form.values.rows];

                        form.setFieldTouched('rows');
                        form.setFieldValue('rows', columns);

                        form.setFieldTouched('columns');
                        form.setFieldValue('columns', rows);
                      }}
                    >
                      <Droppable droppableId="rowColumns">
                        {(droppableProvided, droppableSnapshot) => (
                          <div
                            {...droppableProvided.droppableProps}
                            ref={droppableProvided.innerRef}
                            className={classNames(styles.rowColContainer, {
                              [styles.isDraggingOver]:
                                droppableSnapshot.isDraggingOver,
                            })}
                          >
                            <Draggable draggableId="rows" index={0}>
                              {(draggableProvided, draggableSnapshot) => (
                                <div
                                  {...draggableProvided.draggableProps}
                                  {...draggableProvided.dragHandleProps}
                                  ref={draggableProvided.innerRef}
                                  className={classNames(styles.list, {
                                    [styles.isDragging]:
                                      draggableSnapshot.isDragging,
                                  })}
                                >
                                  <FormFieldSortableList<TableHeadersFormValues>
                                    name="rows"
                                    id="sort-rows"
                                    legend="Rows"
                                    legendSize="s"
                                  />
                                </div>
                              )}
                            </Draggable>
                            <Draggable draggableId="columns" index={1}>
                              {(draggableProvided, draggableSnapshot) => (
                                <div
                                  {...draggableProvided.draggableProps}
                                  {...draggableProvided.dragHandleProps}
                                  ref={draggableProvided.innerRef}
                                  className={classNames(styles.list, {
                                    [styles.isDragging]:
                                      draggableSnapshot.isDragging,
                                  })}
                                >
                                  <FormFieldSortableList<TableHeadersFormValues>
                                    name="columns"
                                    id="sort-columns"
                                    legend="Columns"
                                    legendSize="s"
                                  />
                                </div>
                              )}
                            </Draggable>

                            {droppableProvided.placeholder}
                          </div>
                        )}
                      </Droppable>
                    </DragDropContext>
                  </div>
                </div>
              </FormGroup>

              <Button type="submit">Re-order table</Button>
            </Form>
          );
        }}
      />
    </Details>
  );
};

export default TableHeadersForm;
