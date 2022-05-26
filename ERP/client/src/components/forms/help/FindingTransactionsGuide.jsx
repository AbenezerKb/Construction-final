import { url as host } from "../../../api/api";

function FindingTransactionsGuide() {
    return (
        <div>
            <hr />

            <div>
                <h3>1. If you're looking for an Issue</h3>
                <p>For other transaction use the same steps.</p>
                <img
                    src={`${host}file/screenshot_5feef5c7-5cef-47ad-8c87-b27906648805.jpg`}
                    width="600"
                    alt="If you&#039;re looking for an Issue"
                />
            </div>

            <div>
                <h3>2. Click on Issue Requests</h3>
                <img
                    src={`${host}file/screenshot_04d92f0f-0547-4872-83b9-7484a7a2b2e6.jpg`}
                    width="600"
                    alt="Click on Issue Requests"
                />
            </div>

            <div>
                <h3>3. You can search</h3>
                <img
                    src={`${host}file/screenshot_57ae9d72-6976-4ce1-b892-2a706f4f5f12.jpg`}
                    width="600"
                    alt="You can search"
                />
            </div>

            <div>
                <h3>4. You can click on the headers to sort by their values</h3>
                <img
                    src={`${host}file/screenshot_bd41de42-38e7-4321-a4f9-5de286a9420d.jpg`}
                    width="600"
                    alt="You can click on the headers to sort by their values"
                />
            </div>

            <div>
                <h3>5. You can also navigate through different pages</h3>
                <img
                    src={`${host}file/screenshot_85c642eb-f77a-49e4-b6d9-090ac28be194.jpg`}
                    width="600"
                    alt="You can also navigate through different pages"
                />
            </div>
        </div>
    );
}

export default FindingTransactionsGuide;
