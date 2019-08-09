/* eslint-disable jsx-a11y/no-static-element-interactions,jsx-a11y/click-events-have-key-events */
// @ts-ignore
import ClassicEditor from '@ckeditor/ckeditor5-build-classic';
// @ts-ignore
import CKEditor from '@ckeditor/ckeditor5-react';
import React, { ChangeEvent, createRef, Fragment } from 'react';

import styles from './PrototypeEditableContent.module.scss';

// import 'react-draft-wysiwyg/dist/react-draft-wysiwyg.css';

interface Props {
  editable?: boolean;
  reviewing?: boolean;
  resolveComments?: boolean;
  content: string;
  onContentChange?: (content: string) => void;
}

interface State {
  content: string;
  editing: boolean;
  unsaved: boolean;
}

class PrototypeEditableContent extends React.Component<Props, State> {
  private ref = createRef<HTMLDivElement>();

  private temporaryContent: string = '';

  public static defaultProps = {
    editable: false,
    reviewing: false,
    resolveComments: false,
  };

  public state: State = {
    content: '',
    editing: false,
    unsaved: false,
  };

  public componentDidMount() {
    const { content } = this.props;
    const { editing } = this.state;

    this.setState({
      content,
      editing: false,
      unsaved: false,
    });

    this.temporaryContent = content;

    if (!editing && this.ref.current) {
      this.ref.current.innerHTML = content;
    }
  }

  public componentDidUpdate() {
    const { editing, content } = this.state;
    const { editable } = this.props;

    if (!(editing && editable) && this.ref.current) {
      this.ref.current.innerHTML = content;
    }
  }

  public setEditing = () => {
    const { editable } = this.props;
    const { editing } = this.state;
    if (editable && !editing) {
      this.setState({ editing: true });
    }
  };

  public save = () => {
    this.setState({
      editing: false,
      unsaved: true,
      content: this.temporaryContent,
    });

    const { onContentChange } = this.props;
    if (onContentChange) {
      onContentChange(this.temporaryContent);
    }
  };

  private renderEditableArea(unsaved: boolean, editable?: boolean) {
    return (
      <div>
        <div
          className={
            editable
              ? `${styles.editableContent}  ${unsaved ? styles.unsaved : ''}`
              : ''
          }
          onClick={this.setEditing}
        >
          <div className={styles.editableButton}>
            <div className={styles.editableButtonContent}>
              <span className="govuk-button govuk-button--secondary govuk-body-s govuk-!-margin-bottom-0">
                Edit this section {unsaved ? '- modified' : ''}
              </span>
            </div>
          </div>
          <div className="govuk-!-padding-1" ref={this.ref} />
        </div>
      </div>
    );
  }

  private renderEditor(content: string) {
    return (
      <div>
        <div className={styles.editableContentEditing}>
          <div className={styles.editableButton}>
            <button className="govuk-button" onClick={this.save} type="button">
              Save
            </button>
          </div>
          <CKEditor
            editor={ClassicEditor}
            data={content}
            onChange={(event: ChangeEvent, editor: { getData(): string }) => {
              this.temporaryContent = editor.getData();
            }}
            onInit={(editor: { editing: { view: { focus(): void } } }) => {
              editor.editing.view.focus();
            }}
          />
        </div>
      </div>
    );
  }

  public render() {
    const { editing, content, unsaved } = this.state;

    const { editable } = this.props;

    return (
      <Fragment>
        {editable && editing
          ? this.renderEditor(content)
          : this.renderEditableArea(unsaved, editable)}
      </Fragment>
    );
  }
}

export default PrototypeEditableContent;
