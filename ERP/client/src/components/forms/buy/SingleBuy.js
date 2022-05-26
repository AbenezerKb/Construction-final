import { useEffect, useState, useMemo } from "react"
import { useParams } from "react-router-dom"
import { Form, Container } from "react-bootstrap"
import Item from "../../../models/Item"
import Header from "../../layouts/Header"
import BuyStatusBadge from "./fragments/BuyStatusBadge"
import { ITEMTYPE, BUYSTATUS } from "../../../Constants"
import ApproveBuy from "./ApproveBuy"
import ConfirmBuy from "./ConfirmBuy"
import CheckBuy from "./CheckBuy"
import ViewBuy from "./ViewBuy"
import DeclinedBuy from "./DeclinedBuy"
import { useQuery } from "react-query"
import FormRow from "../../fragments/FormRow"
import { fetchBuy } from "../../../api/buy"
import { useAuth } from "../../../contexts/AuthContext"
import LoadingSpinner from "../../fragments/LoadingSpinner"
import ConnectionError from "../../fragments/ConnectionError"
import AlertNotification from "../../fragments/AlertNotification"

function SingleBuy() {
  const { id } = useParams()
  const [buy, setBuy] = useState({})
  const [addedItems, setAddedItems] = useState([])
  const [isLoading, setIsLoading] = useState(true)
  const [viewOnly, setViewOnly] = useState()
  const [notificationType, setNotificationType] = useState(null)

  const auth = useAuth()

  var query = useQuery(["buy", id], () => fetchBuy(id))

  useEffect(() => {
    if (query.data === undefined) return
    setBuy(query.data)
    let items = []
    for (let buyItem of query.data.buyItems) {
      let itemObj = new Item()

      itemObj.itemId = buyItem.itemId
      itemObj.name = buyItem.item.name
      itemObj.cost = buyItem.cost
      itemObj.type = buyItem.item.type

      itemObj.qtyRequested = buyItem.qtyRequested
      itemObj.requestRemark = buyItem.requestRemark

      if (
        query.data.status >= BUYSTATUS.APPROVED ||
        query.data.status === BUYSTATUS.DECLINED
      ) {
        itemObj.qtyApproved = buyItem.qtyApproved
        itemObj.approveRemark = buyItem.approveRemark
      }

      if (query.data.status >= BUYSTATUS.BOUGHT) {
        itemObj.qtyBought = buyItem.qtyBought
        itemObj.buyRemark = buyItem.buyRemark
      }

      if (buyItem.item.type === ITEMTYPE.MATERIAL) {
        itemObj.spec = buyItem.item.material.spec
        itemObj.units = [buyItem.item.material.unit]
      }

      items.push(itemObj)
    }

    setAddedItems(items)
    setIsLoading(false)
  }, [query.data])

  useEffect(() => {
    if (buy.status === undefined) return
    if (buy.status === BUYSTATUS.QUEUED) {
      setViewOnly(true)
      return setNotificationType(BUYSTATUS.QUEUED)
    }

    if (
      buy.status === BUYSTATUS.REQUESTED &&
      (auth.data.employee.employeeSiteId !== buy.buySiteId ||
        !auth.data.userRole.canCheckBuy)
    ) {
      setViewOnly(true)
      return setNotificationType(BUYSTATUS.REQUESTED)
    }

    if (
      buy.status === BUYSTATUS.CHECKED &&
      (auth.data.employee.employeeSiteId !== buy.buySiteId ||
        !auth.data.userRole.canApproveBuy)
    ) {
      setViewOnly(true)
      return setNotificationType(BUYSTATUS.CHECKED)
    }

    if (
      buy.status === BUYSTATUS.APPROVED &&
      (auth.data.employee.employeeSiteId !== buy.buySiteId ||
        !auth.data.userRole.canConfirmBuy)
    ) {
      setViewOnly(true)
      return setNotificationType(BUYSTATUS.APPROVED)
    }

    setViewOnly(false)
    setNotificationType(null)
  }, [buy])

  function BuyNotificaion() {
    switch (notificationType) {
      case BUYSTATUS.REQUESTED:
        return (
          <AlertNotification
            title="Buy Needs Check"
            content="Buy Has Been Requested and Needs Check to Continue."
          />
        )

      case BUYSTATUS.QUEUED:
        return (
          <AlertNotification
            title="Buy Pending"
            content="Buy Has Been Queued For Purchase, Wait For Purchase."
          />
        )

      case BUYSTATUS.CHECKED:
        return (
          <AlertNotification
            title="Buy Needs Approval"
            content="Buy Has Been Checked and Needs Approval to Continue."
          />
        )

      case BUYSTATUS.APPROVED:
        return (
          <AlertNotification
            title="Buy Confirmation Pending"
            content="Buy Has Been Approved but Hasn't Been Bought Yet."
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
          labelL="Buy Number"
          valueL={buy.buyId}
          labelR="Status"
          valueR={BuyStatusBadge(buy.status)}
        />

        <FormRow
          labelL="Requested By"
          valueL={`${buy.requestedBy.fName} ${buy.requestedBy.mName}`}
          labelR="Requested On"
          valueR={new Date(buy.requestDate).toLocaleString()}
        />
        {buy.status >= BUYSTATUS.CHECKED && (
          <>
            <FormRow
              labelL="Checked By"
              valueL={
                buy.checkedBy
                  ? `${buy.checkedBy.fName} ${buy.checkedBy.mName}`
                  : ""
              }
              labelR="Checked On"
              valueR={
                buy.checkDate ? new Date(buy.checkDate).toLocaleString() : ""
              }
            />
          </>
        )}
        {buy.status >= BUYSTATUS.APPROVED && (
          <>
            <FormRow
              labelL="Approved By"
              valueL={
                buy.approvedBy
                  ? `${buy.approvedBy.fName} ${buy.approvedBy.mName}`
                  : ""
              }
              labelR="Approved On"
              valueR={
                buy.approveDate
                  ? new Date(buy.approveDate).toLocaleString()
                  : ""
              }
            />
          </>
        )}
      </Form>
    )
  }

  const titles = useMemo(
    () => [
      "Declined Buy",
      "Check Buy",
      "Queued Buy",
      "Approve Buy",
      "Confirm Buy",
      "Item Bought",
    ],
    []
  )

  if (query.isError)
    return <ConnectionError status={query.error?.response?.status} />

  if (isLoading) return <LoadingSpinner />

  return (
    <>
      <Header
        title={titles[buy.status]}
        showPrint={buy.status === BUYSTATUS.BOUGHT || viewOnly}
      />

      <Container className="my-3">
        <>
          <TopForm />

          {buy.status === BUYSTATUS.DECLINED && (
            <DeclinedBuy addedItems={addedItems} FormRow={FormRow} buy={buy} />
          )}

          {buy.status === BUYSTATUS.REQUESTED && !viewOnly && (
            <CheckBuy
              addedItems={addedItems}
              setAddedItems={setAddedItems}
              buy={buy}
            />
          )}

          {buy.status === BUYSTATUS.CHECKED && !viewOnly && (
            <ApproveBuy
              addedItems={addedItems}
              setAddedItems={setAddedItems}
              buy={buy}
            />
          )}

          {buy.status === BUYSTATUS.APPROVED && !viewOnly && (
            <ConfirmBuy
              addedItems={addedItems}
              setAddedItems={setAddedItems}
              buy={buy}
            />
          )}

          {(buy.status === BUYSTATUS.BOUGHT ||
            buy.status === BUYSTATUS.QUEUED ||
            viewOnly) && <ViewBuy addedItems={addedItems} buy={buy} />}
        </>
      </Container>
    </>
  )
}

export default SingleBuy
