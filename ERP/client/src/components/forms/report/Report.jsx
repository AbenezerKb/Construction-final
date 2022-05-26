import React from "react";
import { Tabs, Tab } from "react-bootstrap";
import Header from "../../layouts/Header";
import BorrowReport from "./BorrowReport";
import GeneralReport from "./GeneralReport";
import IssueReport from "./IssueReport";
import MaintenanceReport from "./MaintenanceReport";
import PurchaseReport from "./PurchaseReport";
import ReceiveReport from "./ReceiveReport";
import TransferReport from "./TransferReport";

function Report() {
    return (
        <>
            <Header title="Reports" showPrint />
            <Tabs defaultActiveKey="general" className="ms-1 mb-3 mt-2">
                <Tab eventKey="general" title="General">
                    <GeneralReport />
                </Tab>
                <Tab eventKey="purchase" title="Purchase">
                    <PurchaseReport />
                </Tab>
                <Tab eventKey="receive" title="Receive">
                    <ReceiveReport />
                </Tab>
                <Tab eventKey="transfer" title="Transfer">
                    <TransferReport />
                </Tab>
                <Tab eventKey="issue" title="Issue">
                    <IssueReport />
                </Tab>
                <Tab eventKey="borrow" title="Borrow">
                    <BorrowReport />
                </Tab>
                <Tab eventKey="maintenance" title="Maintenance">
                    <MaintenanceReport />
                </Tab>
            </Tabs>
        </>
    );
}

export default Report;
