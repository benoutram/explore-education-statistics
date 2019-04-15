import classNames from 'classnames';
import { UrlLike } from 'next-server/router';
import Link from 'next/link';
import React, { AnchorHTMLAttributes, ReactNode } from 'react';

type Props = {
  children: ReactNode;
  className?: string;
  disabled?: boolean;
  to?: string | UrlLike;
} & AnchorHTMLAttributes<HTMLAnchorElement>;

const ButtonLink = ({
  children,
  className,
  disabled = false,
  to,
  ...props
}: Props) => {
  // We support href and to for backwards
  // compatibility with react-router.
  const href = props.href || to;

  return (
    <Link {...props} href={href}>
      <a
        {...props}
        className={classNames(
          'govuk-button',
          {
            'govuk-button--disabled': disabled,
          },
          className,
        )}
        role="button"
        aria-disabled={disabled}
      >
        {children}
      </a>
    </Link>
  );
};

export default ButtonLink;
