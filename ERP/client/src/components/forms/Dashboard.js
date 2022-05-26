import Header from "../layouts/Header"
import LoadingSpinner from "../fragments/LoadingSpinner"
import { Container, Row, Col } from "react-bootstrap"
import NotificationDashboard from "../notification/NotificationDashboard"
import DashboardCard from "../fragments/DashboardCard"
import maintenanceImg from "../../icons/maintenance.jpg"
import issueImg from "../../icons/issue.jpg"
import borrowImg from "../../icons/borrow.jpg"
import purchaseImg from "../../icons/purchase.jpg"
import receiveImg from "../../icons/receive.jpg"
import buyImg from "../../icons/buy.jpg"
import transferImg from "../../icons/transfer.jpg"
import helpImg from "../../icons/help.jpg"
import reportImg from "../../icons/report.jpg"
import { useAuth } from "../../contexts/AuthContext"

function Dashboard() {
  const { data } = useAuth()

  return (
    <>
      <Header title="Home" showNotification={false} />
      <Container fluid>
        <Row>
          <Col className="overflow-auto" style={{ maxHeight: "92vh" }}>
            <Row className="g-4 mt-2 mx-3" xs={3} xl={4}>
              {data.userRole?.canViewIssue && (
                <Col>
                  <DashboardCard
                    img={issueImg}
                    title="Issue"
                    links={[
                      ...(data.userRole?.canRequestIssue
                        ? [
                            {
                              to: "/issue/new",
                              label: "New Issue Request",
                            },
                          ]
                        : []),
                      { to: "/issue", label: "View Issue Requests" },
                    ]}
                  />
                </Col>
              )}
              {data.userRole?.canViewBorrow && (
                <Col>
                  <DashboardCard
                    img={borrowImg}
                    title="Borrow"
                    links={[
                      ...(data.userRole?.canRequestBorrow
                        ? [
                            {
                              to: "/borrow/new",
                              label: "New Borrow Request",
                            },
                          ]
                        : []),

                      { to: "/borrow", label: "View Borrow Requests" },
                      ...(data.userRole?.canReturnBorrow
                        ? [
                            {
                              to: "/return/new",
                              label: "New Return Request",
                            },
                          ]
                        : []),
                      { to: "/return", label: "View Returns" },
                    ]}
                  />
                </Col>
              )}
              {data.userRole?.canViewTransfer && (
                <Col>
                  <DashboardCard
                    img={transferImg}
                    title="Transfer"
                    links={[
                      ...(data.userRole?.canRequestTransfer
                        ? [
                            {
                              to: "/transfer/material",
                              label: "New Material Transfer",
                            },
                            {
                              to: "/transfer/equipment",
                              label: "New Equipment Transfer",
                            },
                          ]
                        : []),

                      {
                        to: "/transfer/in",
                        label: "View Transfer In Requests",
                      },
                      {
                        to: "/transfer/out",
                        label: "View Transfer Out Requests",
                      },
                    ]}
                  />
                </Col>
              )}

              {data.userRole?.canViewMaintenance && (
                <Col>
                  <DashboardCard
                    img={maintenanceImg}
                    title="Maintenance"
                    links={[
                      ...(data.userRole?.canRequestMaintenance
                        ? [
                            {
                              to: "/maintenance/new",
                              label: "New Maintenance Request",
                            },
                          ]
                        : []),

                      {
                        to: "/maintenance",
                        label: "View Maintenance Requests",
                      },
                    ]}
                  />
                </Col>
              )}
              {data.userRole?.canViewPurchase && (
                <Col>
                  <DashboardCard
                    img={purchaseImg}
                    title="Purchase"
                    links={[
                      ...(data.userRole?.canRequestPurchase
                        ? [
                            {
                              to: "/purchase/material",
                              label: "Purchase Material Request",
                            },
                            {
                              to: "/purchase/equipment",
                              label: "Purchase Equipment Request",
                            },
                          ]
                        : []),
                      {
                        to: "/purchase",
                        label: "View Purchase Requests",
                      },
                      ...(data.userRole?.canRequestBulkPurchase
                        ? [
                            {
                              to: "/bulkPurchase",
                              label: "Bulk Purchase",
                            },
                          ]
                        : []),
                    ]}
                  />
                </Col>
              )}
              {data.userRole?.canViewBuy && (
                <Col>
                  <DashboardCard
                    img={buyImg}
                    title="Buy"
                    links={[
                      ...(data.userRole?.canRequestBuy
                        ? [
                            {
                              to: "/buy/item",
                              label: "Buy An Item",
                            },
                          ]
                        : []),
                      { to: "/buy", label: "View Buy Requests" },
                    ]}
                  />
                </Col>
              )}
              {data.userRole?.canViewReceive && (
                <Col>
                  <DashboardCard
                    img={receiveImg}
                    title="Receive"
                    links={[
                      {
                        to: "/receive/item",
                        label: "Receive Item",
                      },
                      { to: "/receive", label: "View Receives" },
                    ]}
                  />
                </Col>
              )}
              {(data.userRole?.isAdmin || data.userRole?.isFinance) && (
                <Col>
                  <DashboardCard
                    img={reportImg}
                    title="Reports"
                    links={[
                      {
                        to: "/report",
                        label: "Generate Reports",
                      },
                    ]}
                  />
                </Col>
              )}

              <Col>
                <DashboardCard
                  img={helpImg}
                  title="Help"
                  links={[
                    {
                      to: "/help",
                      label: "User's Guide",
                    },
                    {
                      to: "/form-bank",
                      label: "Form Bank",
                    },
                  ]}
                />
              </Col>
            </Row>
          </Col>

          <Col xs={12} lg={4} className="pe-0">
            <NotificationDashboard />
          </Col>
        </Row>
      </Container>
    </>
  )
}

export default Dashboard
