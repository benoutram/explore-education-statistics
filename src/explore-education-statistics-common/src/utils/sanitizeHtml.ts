import sanitize from 'sanitize-html';

const config: sanitize.IOptions = {
  allowedTags: [
    'p',
    'h2',
    'h3',
    'h4',
    'h5',
    'strong',
    'i',
    'a',
    'ul',
    'ol',
    'li',
    'blockquote',
    'figure',
    'table',
    'thead',
    'tbody',
    'tr',
    'td',
    'th',
  ],
  allowedAttributes: {
    figure: ['class'],
    a: ['href'],
    th: ['colspan', 'rowspan'],
    td: ['colspan', 'rowspan'],
  },
};

export default function sanitizeHtml(dirtyHtml: string): string {
  return sanitize(dirtyHtml, config);
}
