/* eslint-disable import/no-duplicates */

// Life is pain. CKEditor still doesn't have type declarations.
// See: https://github.com/ckeditor/ckeditor5/issues/504

// Real CKEditor module declarations (for values that
// we can actually import) should go here.

declare module '@ckeditor/ckeditor5-build-classic' {
  import { EditorClass, EditorConfig } from '@admin/types/ckeditor';

  // https://ckeditor.com/docs/ckeditor5/latest/api/module_editor-classic_classiceditor-ClassicEditor.html
  interface ClassicEditor extends EditorClass {
    new (
      element: string | HTMLElement,
      config: EditorConfig,
    ): ClassicEditorInstance;

    create(element: string | HTMLElement): Promise<ClassicEditorInstance>;
  }

  interface ClassicEditorInstance {
    destroy: () => void;
  }

  const editor: ClassicEditor;
  export default editor;
}

declare module '@ckeditor/ckeditor5-react' {
  import { EditorClass, EditorConfig, Editor } from '@admin/types/ckeditor';
  import { ChangeEvent, ReactElement } from 'react';

  export interface CKEditorProps {
    editor: EditorClass;
    config: EditorConfig;
    data: string;
    // TODO: Don't think this actually emits `ChangeEvent`
    onChange: (event: ChangeEvent, editor: Editor) => void;
    onFocus: () => void;
    onBlur: () => void;
    onReady: (editor: Editor) => void;
  }

  export const CKEditor: (props: CKEditorProps) => ReactElement<CKEditorProps>;
}
