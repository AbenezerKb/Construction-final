import { url as host } from "../../../api/api";

function TransferInGuide() {
    return (
        <div>
            <hr />

            <div>
                <h3>1. A Transfer with a "Sent" status can be Transferred In</h3>
                <img
                    src={`${host}file/screenshot_98532bab-3ea0-454b-ad5f-e054080ceef3.jpg`}
                    width="600"
                    alt='A transfer with a "Sent" status can be Transferred In'
                />
            </div>

            <div>
                <h3>2. Enter the Delivery Person</h3>
                <img
                    src={`${host}file/screenshot_3bc555f2-e043-456f-9b69-2d56d5c7481e.jpg`}
                    width="600"
                    alt="Enter the Delivery Person"
                />
            </div>

            <div>
                <h3>3. Enter The Vehicle Plate Number</h3>
                <img
                    src={`${host}file/screenshot_20742b45-8f4e-4a5b-b59c-59b86f70f93f.jpg`}
                    width="600"
                    alt="Enter The Vehicle Plate Number"
                />
            </div>

            <div>
                <h3>4. Click on Transfer In</h3>
                <img
                    src={`${host}file/screenshot_e8fa6603-2480-49e4-869c-6b94e7c1ed51.jpg`}
                    width="600"
                    alt="Click on Transfer In"
                />
            </div>
        </div>
    );
}

export default TransferInGuide;
