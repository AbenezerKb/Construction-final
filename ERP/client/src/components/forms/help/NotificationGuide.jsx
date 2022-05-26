import { url as host } from "../../../api/api";

function NotificationGuide() {
    return (
        <div>
            <hr />

            <div>
                <h3>1. Notifications appear to let you know something needs your attention</h3>
                <p>You can see your notifications on the home page.</p>
                <img
                    src={`${host}file/screenshot_1b4ae95e-d9f7-439b-8d3f-b102855cc304.jpg`}
                    width="600"
                    alt="Notifications appear to let you know something needs your attention"
                />
            </div>

            <div>
                <h3>2. Notifications are also available on the top right of every page</h3>
                <p>
                    If you're not at the home page you can also click on the bell icon on the top
                    right
                </p>
                <img
                    src={`${host}file/screenshot_ecdf3227-9e1a-44ec-bf47-2ad736780e38.jpg`}
                    width="600"
                    alt="Notifications are also available on the top right of every page"
                />
            </div>

            <div>
                <h3>3. Notification Canvas</h3>
                <img
                    src={`${host}file/screenshot_8eea09bd-b1d9-4942-bef2-e55848adfcb5.jpg`}
                    width="600"
                    alt="Notification Canvas"
                />
            </div>

            <div>
                <h3>4. Clearing Notifications</h3>
                <p>
                    You can clear Notifications by clicking the "-" sign on the top right of each
                    notification. THIS WILL NOT CLEAR THE TRANSACTION. Transaction will be available
                    on its respective list.
                </p>
                <img
                    src={`${host}file/screenshot_30d6fcdd-6b07-4e79-99bb-3d7ef4e2487c.jpg`}
                    width="600"
                    alt="Clearing Notifications"
                />
            </div>

            <div>
                <h3>5. Opening Notifications</h3>
                <p>
                    You can click on each notification to go to the transaction the notification is
                    generated for.
                </p>
                <img
                    src={`${host}file/screenshot_87ef703e-24bc-4af8-b7e9-d4c30e0f4065.jpg`}
                    width="600"
                    alt="Opening Notifications"
                />
            </div>
        </div>
    );
}

export default NotificationGuide;
