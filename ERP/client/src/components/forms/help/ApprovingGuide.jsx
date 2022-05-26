import { url as host } from "../../../api/api";

function ApprovingGuide() {
    return (
        <div>
            <hr />

            <div>
                <h3>1. Before approving a there must be a request.</h3>
                <p>
                    Whenever there's a request you'll get a notification if you can approve it. You
                    can click on the notification to open it.
                </p>
                <img
                    src={`${host}file/screenshot_8a807f06-c804-4181-b410-bf79df4c6897.jpg`}
                    width="600"
                    alt="Before approving a there must be a request."
                />
            </div>

            <div>
                <h3>2. Alternatively you can click on Issues on the left side</h3>
                <img
                    src={`${host}file/screenshot_0970349b-8a17-4200-b1a7-0e4060b3ee37.jpg`}
                    width="600"
                    alt="Alternatively you can click on"
                />
            </div>

            <div>
                <h3>3. Click on Issue Requests</h3>
                <img
                    src={`${host}file/screenshot_fda3027f-be98-4fdd-8701-02fa755422fb.jpg`}
                    width="600"
                    alt="Click on Issue Requests"
                />
            </div>

            <div>
                <h3>4. Open the request you want</h3>
                <img
                    src={`${host}file/screenshot_9e1478ea-a123-4470-8600-1e23325c03bf.jpg`}
                    width="600"
                    alt="Open the request you want"
                />
            </div>

            <div>
                <h3>
                    5. When approving the available qty is displayed on the top right of each Item.
                </h3>
                <p>You cannot approve items that are not available in stock.</p>
                <img
                    src={`${host}file/screenshot_e78ade4f-f957-445b-8e76-88934db690d3.jpg`}
                    width="600"
                    alt="When approving the available qty is displayed on the top right of each Item."
                />
            </div>

            <div>
                <h3>6. Set the quantity you want to approve</h3>
                <img
                    src={`${host}file/screenshot_1d080e4d-2480-47ae-9d81-301469325d2b.jpg`}
                    width="600"
                    alt="Set the quantity you want to approve"
                />
            </div>

            <div>
                <h3>7. Click on Approve</h3>
                <p>
                    After setting the quantities you want to approve click on approve to approve the
                    request. Alternatively you can Decline a request if you deem it unreasonable.
                </p>
                <img
                    src={`${host}file/screenshot_266e9636-42e1-49c0-958d-a575b0b898b1.jpg`}
                    width="600"
                    alt="Click on Approve"
                />
            </div>

            <div>
                <h3>8. Request has been approved.</h3>
                <p>
                    After approving you can see that the status has changed and that the next step
                    is pending.
                </p>
                <img
                    src={`${host}file/screenshot_23bedc74-ee65-4d27-b23c-6dc2cc04c72d.jpg`}
                    width="600"
                    alt="Request has been approved."
                />
            </div>
        </div>
    );
}

export default ApprovingGuide;
