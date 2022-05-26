import { url as host } from "../../../api/api";

function HandingOverGuide() {
    return (
        <div>
            <hr />

            <div>
                <h3>1. First open the transaction</h3>
                <p>
                    Issued or Borrowed Items are Handed Over and transferred Items are Transferred
                    Out in the same manner. The first step is opening the transaction
                </p>
                <img
                    src={`${host}file/screenshot_82b94479-2b75-4145-855e-d3e50c2f0d96.jpg`}
                    width="600"
                    alt="First open the transaction"
                />
            </div>

            <div>
                <h3>2. For equipments asset numbers are set</h3>
                <p>
                    You can select the asset numbers you want from the list. Optionally you can also
                    type the asset numbers to find them.
                </p>
                <img
                    src={`${host}file/screenshot_00f9bd7e-e46c-4a2f-aa41-109cd7bb5c2a.jpg`}
                    width="600"
                    alt="For equipments asset numbers are set"
                />
            </div>

            <div>
                <h3>3. Remaining Items</h3>
                <p>On the right you can see the remaining asset numbers.</p>
                <img
                    src={`${host}file/screenshot_5f71e02d-ef44-4691-a366-1afd9e190c83.jpg`}
                    width="600"
                    alt="Remaining Items"
                />
            </div>

            <div>
                <h3>4. You can also type in any remark</h3>
                <img
                    src={`${host}file/screenshot_41af82a8-c45a-41cd-9134-b35576cb62a0.jpg`}
                    width="600"
                    alt="You can also type in any remark"
                />
            </div>

            <div>
                <h3>
                    5. Click on Hand Over Borrow Request Items to Hand Over / Transfer Out the Items
                </h3>
                <img
                    src={`${host}file/screenshot_80e5d389-651f-4173-8785-44eda2bf49b4.jpg`}
                    width="600"
                    alt="Click on Hand Over Borrow Request Items to Hand Over / Transfer Out the Items"
                />
            </div>

            <div>
                <h3>6. Now the transaction is complete</h3>
                <img
                    src={`${host}file/screenshot_700d7d13-ac25-40a5-9d9b-4bb570b0d72b.jpg`}
                    width="600"
                    alt="Now the transaction is complete"
                />
            </div>
        </div>
    );
}

export default HandingOverGuide;
