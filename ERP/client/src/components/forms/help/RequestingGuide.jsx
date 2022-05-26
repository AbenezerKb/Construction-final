import { url as host } from "../../../api/api";

function RequestingGuide() {
    return (
        <div>
            <hr />

            <div>
                <h3>1. Requesting begins with selecting what to request</h3>
                <p>
                    The steps are similar for any kind of request (Issue, Purchase, Buy, Maintenance
                    and Transfer).
                </p>
                <p>For example for an Issue request click on Issue on the Home page.</p>

                <img
                    src={`${host}file/screenshot_58ade452-1590-4d39-818d-45518881e9fe.jpg`}
                    width="600"
                    alt="Requesting Begins with selecting what to request"
                />
            </div>

            <div>
                <h3>2. Click on New Issue Request</h3>
                <img
                    src={`${host}file/screenshot_68672d6a-1b4d-4f1c-87b6-5b0792a8f9db.jpg`}
                    width="600"
                    alt="Click on New Issue Request"
                />
            </div>

            <div>
                <h3>3. Requesting (Alternate)</h3>
                <p>
                    In other way it is possible to click on Issue on the Left Side which will always
                    be available under Inventory.
                </p>
                <img
                    src={`${host}file/screenshot_add645cc-2e35-45d7-a447-569d2b446e96.jpg`}
                    width="600"
                    alt="Requesting (Alternate)"
                />
            </div>

            <div>
                <h3>4. Click on New Issue Request</h3>
                <img
                    src={`${host}file/screenshot_cf5164b6-29dd-4163-998b-d9ac3156cf0d.jpg`}
                    width="600"
                    alt="Click on New Issue Request"
                />
            </div>

            <div>
                <h3>5. Select An Item</h3>
                <img
                    src={`${host}file/screenshot_4f6241ce-0a0e-4c86-9c16-0c30fb1adf7b.jpg`}
                    width="600"
                    alt="Select An Item"
                />
            </div>

            <div>
                <h3>6. Enter Quantity</h3>
                <img
                    src={`${host}file/screenshot_6aea39b2-3907-4895-9da6-93f689bfc8c9.jpg`}
                    width="600"
                    alt="Enter Quantity"
                />
            </div>

            <div>
                <h3>7. Remark is optional</h3>
                <img
                    src={`${host}file/screenshot_db7ccc60-ce7b-4d5a-86f6-7efbf934bceb.jpg`}
                    width="600"
                    alt="Remark is optional"
                />
            </div>

            <div>
                <h3>8. To add more items click on Add Item</h3>
                <img
                    src={`${host}file/screenshot_a5cff8a4-de3b-47eb-a8c0-3a7ecbf25769.jpg`}
                    width="600"
                    alt="To add more items click on Add Item"
                />
            </div>

            <div>
                <h3>9. Select Item and enter Quantity</h3>
                <p>You can remove Items by clicking on Remove Item.</p>
                <img
                    src={`${host}file/screenshot_480cf2f2-185a-48a1-a57c-02cad04591d4.jpg`}
                    width="600"
                    alt="Select Item and enter Quantity"
                />
            </div>

            <div>
                <h3>10. Click on Request Issue</h3>
                <p>After you have added all the Items you need then click on Request.</p>
                <img
                    src={`${host}file/screenshot_70fc9369-d0db-47b6-90f3-bd4783a611c3.jpg`}
                    width="600"
                    alt="Click on Request Issue"
                />
            </div>

            <div>
                <h3>11. Then you wait...</h3>
                <p>
                    After requesting the items you will have to wait for the items to be approved
                    and handed over to you. You can see the status of the request at any point by
                    selecting this Issue from the Issues List.
                </p>
                <img
                    src={`${host}file/screenshot_9444bf67-86b5-43c3-8b36-adf6ee00854e.jpg`}
                    width="600"
                    alt="Then you wait..."
                />
            </div>

            <div>
                <h3>A. For a borrow Request you can select the category first</h3>
                <img
                    src={`${host}file/screenshot_e64ec9cd-d2ed-4318-a010-9b7b49ebd9ec.jpg`}
                    width="600"
                    alt="For a borrow Request you can select the category first"
                />
            </div>

            <div>
                <h3>B. After selecting the category the steps are the same.</h3>
                <p>
                    If you want to change the category you can click on change category or change
                    the category from the drop down on each equipment.
                </p>
                <img
                    src={`${host}file/screenshot_7536c3c2-af32-4bbb-b3da-a8074daf331d.jpg`}
                    width="600"
                    alt="After selecting the category the steps are the same."
                />
            </div>
        </div>
    );
}

export default RequestingGuide;
