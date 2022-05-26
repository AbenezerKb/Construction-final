import Header from "../../layouts/Header";
import { Link } from "react-router-dom";
import UserGuideLayout from "./UserGuideLayout";
import GeneralReportGuide from "./GeneralReportGuide";
import CategoricalReportGuide from "./CategoricalReportGuide";
import Test from "./Test";
import RequestingGuide from "./RequestingGuide";
import NotificationGuide from "./NotificationGuide";
import ApprovingGuide from "./ApprovingGuide";
import HandingOverGuide from "./HandingOverGuide";
import TransferInGuide from "./TransferInGuide";
import FindingTransactionsGuide from "./FindingTransactionsGuide";
import MinimumStockGuide from "./MinimumStockGuide";

function UserGuide() {
    return (
        <>
            <Header title="User Guide" showPrint />

            <UserGuideLayout>
                <FindingTransactionsGuide title="Finding Transactions" />
                <RequestingGuide title="Requesting" />
                <ApprovingGuide title="Approving" />
                <HandingOverGuide title="Handing Over / Transferring Out" />
                <TransferInGuide title="Transferring In" />
                <NotificationGuide title="Notifications" />
                <GeneralReportGuide title="Generating General Report" />
                <CategoricalReportGuide title="Generating Categorical Report" />
                <MinimumStockGuide title="Setting Minimum Stock" />
            </UserGuideLayout>
        </>
    );
}

export default UserGuide;
