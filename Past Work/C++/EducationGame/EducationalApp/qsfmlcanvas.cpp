#include "qsfmlcanvas.h"
// Platform-specific headers
#ifdef Q_WS_X11
#include <Qt/qx11info_x11.h>
#include <X11/Xlib.h>
#endif

QSFMLCanvas::QSFMLCanvas(QWidget* Parent, const QPoint& Position, const QSize& Size, unsigned int FrameTime)
: QWidget(Parent), isInitialized (false)
{
    // To allow direct rendering into the widget
    setAttribute(Qt::WA_PaintOnScreen);
    setAttribute(Qt::WA_OpaquePaintEvent);
    setAttribute(Qt::WA_NoSystemBackground);

    // To enable keyboard events to be received
    setFocusPolicy(Qt::StrongFocus);

    // Setup the widget geometry
    move(Position);
    resize(Size);

    // Setup the timer
    qTimer.setInterval(FrameTime);
}

QSFMLCanvas::~QSFMLCanvas()
{
}

void QSFMLCanvas::showEvent(QShowEvent*)
{
    if (!isInitialized)
    {
        // Under X11, we need to flush the commands sent to the server to ensure that
        // SFML will get an updated view of the windows
#ifdef Q_WS_X11
        XFlush(QX11Info::display());
#endif

        // Create the SFML window with the widget handle

        // for cross platform compatibility
#if defined(_WIN32) || defined(__APPLE__)
        RenderWindow::create(reinterpret_cast<sf::WindowHandle>(winId()));
#else
        RenderWindow::create((winId()));
#endif
        // Let the derived class do its specific stuff
        OnInit();
        // Setup the timer to trigger a refresh at specified framerate
        connect(&qTimer, SIGNAL(timeout()), this, SLOT(repaint()));
        qTimer.start();
        isInitialized = true;
    }
}

QPaintEngine* QSFMLCanvas::paintEngine() const
{
    return 0;
}

void QSFMLCanvas::paintEvent(QPaintEvent*)
{
    // Let the derived class do its specific stuff
    OnUpdate();
    // Display on screen
    RenderWindow::display();
}

void QSFMLCanvas::OnInit()
{
}

void QSFMLCanvas::OnUpdate()
{
}

// API reference:
// https://github.com/Gnuk/QSFMLproject
