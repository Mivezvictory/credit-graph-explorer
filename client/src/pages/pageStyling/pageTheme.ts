export const LOGIN_PAGE_STYLING = {
    position: 'relative', // changed from 'relative' to 'fixed'
    left: 0,
    top: 0,
    width: '100vw',
    height: '100vh',
    backgroundImage: "url('/images/bg-image.jpg')",
    backgroundSize: 'cover',
    backgroundPosition: 'center',
    backgroundRepeat: 'no-repeat',
    backgroundAttachment: 'fixed',
    display: 'flex',
    alignItems: 'center',
    justifyContent: 'center',
    overflow: 'hidden',
    zIndex: 0, // ensure background stays behind overlay and card
}

export const SEMI_TRANSPARENT_OVERLAY = {
    position: 'absolute',
    inset: 0,
    backgroundColor: 'rgba(0, 0, 0, 0.4)',
    zIndex: 1,
}

export const SINGIN_CARD_STYLING = {
    zIndex: 2,
    height: '80vh',
    display: 'flex',
    flexDirection: 'column',
    alignItems: 'center',
    justifyContent: 'center',
    bgcolor: 'rgba(34, 41, 43, 0.6)',//rgba(96, 119, 127, 0.6)',
    color: 'white',
    p: 4,
    borderRadius: 2,
    boxShadow: 1,
}