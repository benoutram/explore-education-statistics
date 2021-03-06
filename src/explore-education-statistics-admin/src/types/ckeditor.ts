// Declare our own custom CKEditor types that aren't
// exposed by any of the packages that we consume.

// https://ckeditor.com/docs/ckeditor5/latest/api/module_core_editor_editor-Editor.html
export interface EditorClass {
  new (config: EditorConfig): Editor;
}

export interface Editor {
  plugins: PluginCollection;
  editing: {
    view: {
      focus(): void;
    };
  };
  model: Model;
  getData(): string;
}

export interface EditorConfig {
  toolbar: string[];
  extraPlugins?: Plugin[];
  image?: {
    toolbar: string[];
    resizeOptions?: ResizeOption[];
  };
  table?: {
    contentToolbar?: string[];
  };
  heading?: {
    options: HeadingOption[];
  };
}

export interface PluginCollection {
  get<T extends Plugin>(key: string): T;
}

// eslint-disable-next-line @typescript-eslint/no-empty-interface
export interface Plugin {}

export interface HeadingOption {
  model:
    | 'heading1'
    | 'heading2'
    | 'heading3'
    | 'heading4'
    | 'heading5'
    | 'paragraph';
  view?: 'h1' | 'h2' | 'h3' | 'h4' | 'h5';
  title: string;
  class?: string;
}

export interface ResizeOption {
  name: string;
  value: string | null;
  label: string;
  icon?: string;
}

// https://ckeditor.com/docs/ckeditor5/latest/framework/guides/deep-dive/upload-adapter.html#the-anatomy-of-the-adapter
export interface UploadAdapter {
  upload(): Promise<ImageUploadResult>;
  abort(): void;
}

export interface ImageUploadResult {
  /**
   * The default url for the image that
   * was uploaded to the server.
   */
  default: string;

  /**
   * Additional urls can be added for different
   * image sizes. This allows us to show more appropriately
   * sized images for the user's screen size.
   *
   * Ideally, we should resize images to multiple
   * sizes for an optimized user experience.
   */
  [size: string]: string;
}

// https://ckeditor.com/docs/ckeditor5/latest/api/module_engine_model_model-Model.html
export interface Model {
  readonly document: Document;
}

// https://ckeditor.com/docs/ckeditor5/latest/api/module_engine_model_document-Document.html
export interface Document {
  getRoot(name: string): RootElement;
}

export interface Node {
  readonly endOffset: number | null;
  readonly index: number | null;
  readonly isEmpty: boolean;
  readonly nextSibling: Node | null;
  readonly offsetSize: number;
  readonly parent: Element | null;
  readonly previousSibling: Node | null;
  readonly root: Node;
  readonly startOffset: number | null;

  getAttribute<T>(key: string): T | undefined;
  getAttributeKeys(): Iterable<string>;
  hasAttribute(key: string): boolean;
}

export interface Element extends Node {
  readonly name: string;

  getChild(index: number): Node;
  getChildren(): Iterable<Node>;
}

export interface RootElement extends Element {
  readonly document: Document;

  getChild(index: number): Element;
  getChildren(): Iterable<Element>;
}
