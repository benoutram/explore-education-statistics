import { useEffect, useState } from 'react';

/**
 * Hook that detects if the window currently
 * matches a given media {@param query} string.
 */
export default function useMedia(query: string) {
  const [isMedia, setMedia] = useState(false);

  useEffect(() => {
    let mounted = true;

    const onChange = (event: MediaQueryListEvent) => {
      if (!mounted) {
        return;
      }

      setMedia(event.matches);
    };

    const mediaQueryList = window.matchMedia(query);
    setMedia(mediaQueryList.matches);

    mediaQueryList.addEventListener('change', onChange);

    return () => {
      mounted = false;
      mediaQueryList.removeEventListener('change', onChange);
    };
  }, [query]);

  return {
    isMedia,
    onMedia<T, R>(value: T, defaultValue?: R): T | R | undefined {
      return isMedia ? value : defaultValue;
    },
  };
}

export const useDesktopMedia = () => useMedia('(min-width: 40.0625em)');
