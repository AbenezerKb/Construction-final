class Item {
  constructor() {
    this.itemId = 0
    this.name = ""
    this.type = 1
    this.spec = ""
    this.description = ""
    this.unit = 0
    this.units = []
    this.qtyRequested = 0
    this.qtyApproved = 0
    this.qtyBought = 0
    this.qtyPurchased = 0
    this.qtySent = 0
    this.qtyReceived = 0
    this.qtyAvailable = 0
    this.cost = 0
    this.totalCost = 0
    this.total = 0
    this.approveRemark = ""
    this.buyRemark = ""
    this.requestRemark = ""
    this.sendRemark = ""
    this.receiveRemark = ""
    this.purchaseRemark = ""
    this.handRemark = ""
    this.fixRemark = ""
    this.reason = ""
    this.returnRemark = ""
    this.assetDamageId = "-1"
    this.file = null
    this.fileName = ""

    this.equipmentModelId = 0

    this.isSetAssets = false
    this.equipmentAssets = []

    this.key = Math.random()
  }

  load() {
    //set id and use this get units
    this.units = ["Cubic Meter", "Cubic Centimeter"]
    this.qtyAvailable = 10
  }

  clear() {
    //set id and use this get units
    this.units = []
    this.qtyAvailable = 0
  }
}

export default Item
