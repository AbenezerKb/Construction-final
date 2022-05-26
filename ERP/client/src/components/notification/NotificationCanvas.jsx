import { Button, Offcanvas, Tooltip, OverlayTrigger, Badge } from "react-bootstrap";
import { useEffect, useState } from "react";
import { FaBell } from "react-icons/fa";
import SingleNotification from "./SingleNotification";
import { useQuery } from "react-query";
import { fetchNotifications, fetchNow } from "../../api/notification";
import { NOTIFICATIONROUTE, NOTIFICATIONTYPE } from "../../Constants";

function NotificationCanvas() {
    const [notificationCount, setNotificationCount] = useState(0);
    const [now, setNow] = useState("");
    const [showCanvas, setShowCanvas] = useState(false);

    var notificationQuery = useQuery(["notifications"], fetchNotifications);
    var nowQuery = useQuery(["now"], fetchNow);

    useEffect(() => {
        setNow(nowQuery.data?.data);
    });

    useEffect(() => {
        setNotificationCount(notificationQuery?.data?.length);
    }, [notificationQuery.data]);

    const handleClose = () => setShowCanvas(false);
    const handleShow = () => setShowCanvas(true);

    const renderTooltip = (props) => (
        <Tooltip id="button-tooltip" {...props}>
            {notificationCount ? `${notificationCount} Notifications` : "No New Notifications"}
        </Tooltip>
    );

    return (
        <>
            <OverlayTrigger
                placement="left"
                delay={{ show: 250, hide: 400 }}
                overlay={renderTooltip}
            >
                <Button className="p-1" variant="transparent" onClick={handleShow}>
                    <FaBell className="text-white fs-4" />
                    {notificationCount ? (
                        <Badge
                            pill
                            bg="danger"
                            style={{ position: "absolute", marginLeft: "-9px", marginTop: "15px" }}
                        >
                            {notificationCount}
                        </Badge>
                    ) : (
                        ""
                    )}
                </Button>
            </OverlayTrigger>

            <Offcanvas show={showCanvas} onHide={handleClose} placement="end">
                <Offcanvas.Header closeButton>
                    <Offcanvas.Title>Notifications</Offcanvas.Title>
                </Offcanvas.Header>
                <Offcanvas.Body>
                    {!notificationCount && (
                        <h1 className="display-6 fs-5 text-center mt-2">No New Notifications</h1>
                    )}

                    {notificationQuery.data?.map((notification) => (
                        <SingleNotification
                            key={notification.notificationId}
                            title={notification.title}
                            notificationId={notification.notificationId}
                            content={notification.content}
                            date={notification.date}
                            now={now || ""}
                            target={
                                notification.type === NOTIFICATIONTYPE.MINEQUIPMENT ||
                                notification.type === NOTIFICATIONTYPE.MINMATERIAL
                                    ? `${NOTIFICATIONROUTE[notification.type]}`
                                    : `${NOTIFICATIONROUTE[notification.type]}/${
                                          notification.actionId
                                      }`
                            }
                        />
                    ))}
                </Offcanvas.Body>
            </Offcanvas>
        </>
    );
}

export default NotificationCanvas;
