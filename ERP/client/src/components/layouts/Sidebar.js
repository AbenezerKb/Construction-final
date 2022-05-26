import { useState } from "react"
import {
  ProSidebar,
  Menu,
  MenuItem,
  SubMenu,
  SidebarHeader,
  SidebarFooter,
  SidebarContent,
} from "react-pro-sidebar"
import "react-pro-sidebar/dist/css/styles.css"
import { GoSettings } from "react-icons/go"
import { FaHome, FaList, FaBars, FaUser, FaChartBar } from "react-icons/fa"
import { Link, useNavigate } from "react-router-dom"
import { useAuth, useAuthUpdate } from "../../contexts/AuthContext"

function Sidebar() {
  const navigate = useNavigate()
  const updateAuthContext = useAuthUpdate()

  const { data } = useAuth()

  const [collapsed, setCollapsed] = useState(false)

  return (
    <>
      <ProSidebar
        className="text-light sidebar-decrease-padding"
        collapsed={collapsed}
        width="280px"
        style={{ height: "100vh", position: "sticky", top: 0 }}
      >
        <SidebarHeader className="p-1 text-nowrap">
          <MenuItem
            className="ms-4 me-3"
            icon={<FaBars className="fs-5" />}
            style={{ display: "inline-flex" }}
            onClick={() => setCollapsed(!collapsed)}
          />
          {!collapsed && "ERP"}
        </SidebarHeader>
        <SidebarContent>
          <Menu>
            <MenuItem className="" icon={<FaHome className="fs-5" />}>
              Home <Link to="/" />
            </MenuItem>

            <SubMenu title="Setting" icon={<GoSettings className="fs-5" />}>
              {data.userRole.canEditUser && (
                <SubMenu title="Employee">
                  <MenuItem>
                    Manage Employees <Link to="/employee" />
                  </MenuItem>
                </SubMenu>
              )}
              <SubMenu title="Role">
                <MenuItem>
                  Add a new Role <Link to="/role/new" />
                </MenuItem>
                <MenuItem>
                  Edit Role <Link to="/role" />
                </MenuItem>
              </SubMenu>
              <SubMenu title="Site">
                <MenuItem>
                  Add a new Site <Link to="/site/new" />
                </MenuItem>
                <MenuItem>
                  Edit Site <Link to="/site" />
                </MenuItem>
              </SubMenu>
              <SubMenu title="Stock">
                <MenuItem>
                  Add a new Material <Link to="/item/material/new" />
                </MenuItem>
                <MenuItem>
                  Edit Material <Link to="/item/material" />
                </MenuItem>
                <SubMenu title="Equipment">
                  <MenuItem>
                    Add a new Equipment <Link to="/item/equipment/new" />
                  </MenuItem>
                  <MenuItem>
                    Add a new Model <Link to="/item/equipment/model/new" />
                  </MenuItem>
                  <MenuItem>
                    Add a new Category{" "}
                    <Link to="/item/equipment/category/new" />
                  </MenuItem>
                  <MenuItem>
                    Edit Equipment <Link to="/item/equipment" />
                  </MenuItem>
                  <MenuItem>
                    Edit Model <Link to="/item/equipment/model" />
                  </MenuItem>
                </SubMenu>

                <MenuItem>
                  Minimum Stock <Link to="/item/minimum-stock" />
                </MenuItem>
                <MenuItem>
                  Manage Damages <Link to="/item/damage" />
                </MenuItem>
                <MenuItem>
                  Import Asset Numbers <Link to="/item/import-asset-numbers" />
                </MenuItem>
              </SubMenu>

              <MenuItem>
                Notifications <Link to="/notification" />
              </MenuItem>
              <MenuItem>
                Company Name / Prefix <Link to="/misc/company" />
              </MenuItem>
            </SubMenu>

            <SubMenu
              title="Inventory"
              icon={<FaList className="fs-5" />}
              defaultOpen={true}
            >
              {data.userRole.canViewBuy && (
                <SubMenu title="Buy">
                  {data.userRole?.canRequestBuy && (
                    <MenuItem>
                      Buy an Item <Link to="/buy/item" />
                    </MenuItem>
                  )}
                  <MenuItem>
                    Buy Requests <Link to="/buy" />
                  </MenuItem>
                </SubMenu>
              )}
              {data.userRole.canViewPurchase && (
                <SubMenu title="Purchase">
                  {data.userRole?.canRequestPurchase && (
                    <MenuItem>
                      Purchase Material
                      <Link to="/purchase/material" />
                    </MenuItem>
                  )}
                  {data.userRole?.canRequestPurchase && (
                    <MenuItem>
                      Purchase Equipment
                      <Link to="/purchase/equipment" />
                    </MenuItem>
                  )}

                  <MenuItem>
                    List Purchase Requests <Link to="/purchase" />
                  </MenuItem>
                  {data.userRole?.canRequestBulkPurchase && (
                    <MenuItem>
                      Bulk Purchase
                      <Link to="/bulkPurchase" />
                    </MenuItem>
                  )}
                </SubMenu>
              )}

              <MenuItem>
                Notifications <Link to="/notification" />
              </MenuItem>
              <MenuItem>
                Company Name / Prefix <Link to="/misc/company" />
              </MenuItem>
            </SubMenu>

            <SubMenu
              title="Inventory"
              icon={<FaList className="fs-5" />}
              defaultOpen={true}
            >
              <SubMenu title="Buy">
                {data.userRole?.canRequestBuy && (
                  <MenuItem>
                    Buy an Item <Link to="/buy/item" />
                  </MenuItem>
                )}
                <MenuItem>
                  Buy Requests <Link to="/buy" />
                </MenuItem>
              </SubMenu>

              {data.userRole.canViewPurchase && (
                <SubMenu title="Purchase">
                  {data.userRole?.canRequestPurchase && (
                    <MenuItem>
                      Purchase Material
                      <Link to="/purchase/material" />
                    </MenuItem>
                  )}
                  {data.userRole?.canRequestPurchase && (
                    <MenuItem>
                      Purchase Equipment
                      <Link to="/purchase/equipment" />
                    </MenuItem>
                  )}

                  <MenuItem>
                    List Purchase Requests <Link to="/purchase" />
                  </MenuItem>
                  {data.userRole?.canRequestPurchase && (
                    <MenuItem>
                      Bulk Purchase
                      <Link to="/bulkPurchase" />
                    </MenuItem>
                  )}
                </SubMenu>
              )}

              {data.userRole.canViewReceive && (
                <SubMenu title="Receive">
                  {data.userRole?.canReceive && (
                    <MenuItem>
                      Receive an Item <Link to="/receive/item" />
                    </MenuItem>
                  )}

                  <MenuItem>
                    List Received Items <Link to="/receive" />
                  </MenuItem>
                </SubMenu>
              )}

              {data.userRole.canViewTransfer && (
                <SubMenu title="Transfer">
                  {data.userRole?.canRequestTransfer && (
                    <>
                      <MenuItem>
                        New Material Transfer
                        <Link to="/transfer/material" />
                      </MenuItem>
                      <MenuItem>
                        New Equipment Transfer
                        <Link to="/transfer/equipment" />
                      </MenuItem>
                    </>
                  )}
                  <MenuItem>
                    Transfer In <Link to="/transfer/in" />
                  </MenuItem>
                  <MenuItem>
                    Transfer Out
                    <Link to="/transfer/out" />
                  </MenuItem>
                </SubMenu>
              )}

              {data.userRole.canViewIssue && (
                <SubMenu title="Issue">
                  {data.userRole.canRequestIssue && (
                    <MenuItem>
                      New Issue Request
                      <Link to="/issue/new" />
                    </MenuItem>
                  )}
                  <MenuItem>
                    Issue Requests
                    <Link to="/issue" />
                  </MenuItem>
                </SubMenu>
              )}

              {data.userRole.canViewBorrow && (
                <SubMenu title="Borrow">
                  {data.userRole.canRequestBorrow && (
                    <MenuItem>
                      New Borrow Request <Link to="/borrow/new" />
                    </MenuItem>
                  )}
                  <MenuItem>
                    Borrow Requests <Link to="/borrow" />
                  </MenuItem>

                  {data.userRole.canReturnBorrow && (
                    <MenuItem>
                      Return Assets <Link to="/return/new" />
                    </MenuItem>
                  )}

                  <MenuItem>
                    Returned Assets <Link to="/return" />
                  </MenuItem>
                </SubMenu>
              )}

              {data.userRole.canViewMaintenance && (
                <SubMenu title="Maintenance">
                  {data.userRole.canRequestMaintenance && (
                    <MenuItem>
                      New Maintenance Request <Link to="/maintenance/new" />
                    </MenuItem>
                  )}
                  <MenuItem>
                    Maintenance Requests <Link to="/maintenance" />
                  </MenuItem>
                </SubMenu>
              )}
              {data.userRole.isAdmin && (
                <>
                  <SubMenu title="Rent">
                    <MenuItem>
                      New Rent Request
                      <Link to="/rent/new" />
                    </MenuItem>
                  </SubMenu>

                  <SubMenu title="Garage">
                    <MenuItem>
                      New Maintenance Request
                      <Link to="/garage/new" />
                    </MenuItem>
                  </SubMenu>
                  <SubMenu title="Equipment Production">
                    <MenuItem>
                      New Production Request
                      <Link to="/production/new" />
                    </MenuItem>
                  </SubMenu>
                </>
              )}
            </SubMenu>

            {(data.userRole.isAdmin || data.userRole.isFinance) && (
              <MenuItem icon={<FaChartBar className="fs-5" />}>
                Reports <Link to="/report" />
              </MenuItem>
            )}
          </Menu>
        </SidebarContent>
        <SidebarFooter>
          <Menu subMenuBullets={true}>
            <SubMenu title={data.username} icon={<FaUser className="fs-5" />}>
              <MenuItem
                onClick={() => {
                  localStorage.removeItem("token")
                  localStorage.removeItem("data")
                  updateAuthContext({ token: "", data: "", username: "" })
                  navigate("/login")
                }}
              >
                Logout
              </MenuItem>
            </SubMenu>
          </Menu>
        </SidebarFooter>
      </ProSidebar>
    </>
  )
}

export default Sidebar
