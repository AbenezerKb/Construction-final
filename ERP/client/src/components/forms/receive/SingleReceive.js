import { useEffect, useState, useMemo } from "react"
import { useParams } from "react-router-dom"
import { Form, Container } from "react-bootstrap"
import Item from "../../../models/Item"
import Header from "../../layouts/Header"
import ReceiveStatusBadge from "./fragments/ReceiveStatusBadge"
import { ITEMTYPE, RECEIVESTATUS } from "../../../Constants"
import ApproveReceive from "./ApproveReceive"
import RequestReceive from "./RequestReceive"
import ViewReceive from "./ViewReceive"
import { useQuery } from "react-query"
import FormRow from "../../fragments/FormRow"
import { fetchReceive } from "../../../api/receive"
import { useAuth } from "../../../contexts/AuthContext"
import LoadingSpinner from "../../fragments/LoadingSpinner"
import ConnectionError from "../../fragments/ConnectionError"
import AlertNotification from "../../fragments/AlertNotification"

function SingleReceive() {
  const { id } = useParams()
  const [receive, setReceive] = useState({})
  const [addedItems, setAddedItems] = useState([])
  const [isLoading, setIsLoading] = useState(true)
  const [viewOnly, setViewOnly] = useState()
  const [notificationType, setNotificationType] = useState(null)

  const auth = useAuth()

  var query = useQuery(["receive", id], () => fetchReceive(id))

  useEffect(() => {
    if (query.data === undefined) return
    setReceive(query.data)
    let items = []
    for (let receiveItem of query.data.receiveItems) {
      let itemObj = new Item()

      itemObj.itemId = receiveItem.itemId
      itemObj.name = receiveItem.item.name
      itemObj.cost = receiveItem.cost
      itemObj.type = receiveItem.item.type

      if (query.data.status >= RECEIVESTATUS.RECEIVED) {
        itemObj.qtyReceived = receiveItem.qtyReceived
        itemObj.receiveRemark = receiveItem.receiveRemark
      }

      if (receiveItem.item.type === ITEMTYPE.MATERIAL) {
        itemObj.spec = receiveItem.item.material.spec
        itemObj.units = [receiveItem.item.material.unit]
      }

      if (receiveItem.item.type === ITEMTYPE.EQUIPMENT) {
        itemObj.description = receiveItem.item.equipment.description
        itemObj.units = [receiveItem.item.equipment.unit]
        itemObj.equipmentModelId = receiveItem.equipmentModelId
      }

      items.push(itemObj)
    }

    setAddedItems(items)
    setIsLoading(false)
  }, [query.data])

  useEffect(() => {
    if (receive.status === undefined) return

    if (
      receive.status === RECEIVESTATUS.PURCHASED &&
      (auth.data.employee.employeeSiteId !== receive.receivingSiteId ||
        !auth.data.userRole.canReceive)
    ) {
      setViewOnly(true)
      return setNotificationType(RECEIVESTATUS.PURCHASED)
    }

    if (
      receive.status === RECEIVESTATUS.RECEIVED &&
      (auth.data.employee.employeeSiteId !== receive.receivingSiteId ||
        !auth.data.userRole.canApproveReceive)
    ) {
      setViewOnly(true)
      return setNotificationType(RECEIVESTATUS.RECEIVED)
    }

    setViewOnly(false)
    setNotificationType(null)
  }, [receive])

  function ReceiveNotificaion() {
    switch (notificationType) {
      case RECEIVESTATUS.PURCHASED:
        return (
          <AlertNotification
            title="Purchase Needs Receive"
            content="Purchase Has Been Made And Needs To Be Received."
          />
        )

      case RECEIVESTATUS.RECEIVED:
        return (
          <AlertNotification
            title="Purchase Needs Check"
            content="Purchase Has Been Received and Needs Check to Continue."
          />
        )

      default:
        return <></>
    }
  }

  function TopForm() {
    return (
      <Form>
        <FormRow
          labelL="Receive Number"
          valueL={receive.receiveId}
          labelR="Status"
          valueR={ReceiveStatusBadge(receive.status)}
        />

        <FormRow
          labelL="Purchase Reference Number"
          valueL={receive.purchaseId}
          labelR="Purchased On"
          valueR={new Date(receive.purchaseDate).toLocaleString()}
        />

        {receive.status >= RECEIVESTATUS.RECEIVED && (
          <>
            <FormRow
              labelL="Received By"
              valueL={
                receive.receivedBy
                  ? `${receive.receivedBy.fName} ${receive.receivedBy.mName}`
                  : ""
              }
              labelR="Received On"
              valueR={
                receive.receiveDate
                  ? new Date(receive.receiveDate).toLocaleString()
                  : ""
              }
            />
          </>
        )}
        {receive.status >= RECEIVESTATUS.APPROVED && (
          <>
            <FormRow
              labelL="Approved By"
              valueL={
                receive.approvedBy
                  ? `${receive.approvedBy.fName} ${receive.approvedBy.mName}`
                  : ""
              }
              labelR="Approved On"
              valueR={
                receive.approveDate
                  ? new Date(receive.approveDate).toLocaleString()
                  : ""
              }
            />
          </>
        )}
      </Form>
    )
  }

  const titles = useMemo(
    () => ["", "Receive Item", "Approve Receive", "Approved Receive"],
    []
  )

  if (isLoading) return <LoadingSpinner />

  if (query.isError)
    return <ConnectionError status={query.error?.response?.status} />

  return (
    <>
      <Header
        title={titles[receive.status]}
        showPrint={receive.status === RECEIVESTATUS.APPROVED || viewOnly}
      />

      <Container className="my-3">
        <>
          <ReceiveNotificaion />

          <TopForm />

          {receive.status === RECEIVESTATUS.PURCHASED && !viewOnly && (
            <RequestReceive
              addedItems={addedItems}
              setAddedItems={setAddedItems}
              receive={receive}
            />
          )}

          {receive.status === RECEIVESTATUS.RECEIVED && !viewOnly && (
            <ApproveReceive
              addedItems={addedItems}
              setAddedItems={setAddedItems}
              receive={receive}
            />
          )}

          {(receive.status === RECEIVESTATUS.APPROVED || viewOnly) && (
            <ViewReceive addedItems={addedItems} receive={receive} />
          )}
        </>
      </Container>
    </>
  )
}

export default SingleReceive
