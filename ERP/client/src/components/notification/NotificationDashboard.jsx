import { Badge } from "react-bootstrap";
import { useEffect, useState } from "react";
import { FaBell } from "react-icons/fa";
import SingleNotification from "./SingleNotification";
import { useQuery } from "react-query";
import { fetchNotifications, fetchNow } from "../../api/notification";
import { NOTIFICATIONROUTE, NOTIFICATIONTYPE } from "../../Constants";
import LoadingSpinner from "../fragments/LoadingSpinner";
import ConnectionError from "../fragments/ConnectionError";

function NotificationDashboard() {
    const [notificationCount, setNotificationCount] = useState(0);
    const [now, setNow] = useState("");

    var notificationQuery = useQuery(["notifications"], fetchNotifications);
    var nowQuery = useQuery(["now"], fetchNow);

    useEffect(() => {
        setNow(nowQuery.data?.data);
    });

    useEffect(() => {
        setNotificationCount(notificationQuery?.data?.length);
    }, [notificationQuery.data]);

    if (notificationQuery.isError || nowQuery.isError)
        return (
            <ConnectionError
                status={
                    notificationQuery?.error?.response?.status ?? nowQuery?.error?.response?.status
                }
            />
        );

    if (
        notificationQuery.isLoading ||
        notificationQuery.isFetching ||
        nowQuery.isFetching ||
        nowQuery.isLoading
    )
        return <LoadingSpinner />;

    return (
        <div className="mt-3">
            <h1 className="display-6 fs-4 text-center mb-3">
                Notifications
                <span className=" ms-2 fs-5 align-top">
                    {notificationCount ? (
                        <Badge pill bg="danger">
                            {notificationCount}
                        </Badge>
                    ) : (
                        ""
                    )}
                </span>
            </h1>
            <div className="overflow-auto" style={{ maxHeight: "83vh" }}>
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
                                : `${NOTIFICATIONROUTE[notification.type]}/${notification.actionId}`
                        }
                    />
                ))}
            </div>
        </div>
    );
}

export default NotificationDashboard;
