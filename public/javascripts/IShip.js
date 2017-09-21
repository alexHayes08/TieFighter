class IShip {
    constructor (shipClassification, shipName, maxHealth) { 
        this.shipClassification = shipClassification;
        this.shipName = shipName;
        this.maxHealth = maxHealth;
    }
    get cameras ()  {
        return {
            firstPerson: {},
            thirdPerson: {}
        }
    }
    get maxHealth () {
        return this.maxHealth;
    }
    get shipName () {
        return this.shipName;
    }
    takeDamage () {

    }
}

class IAcceleratable {
    constructor (linearAcceleration, angularAccleration) {
        if (!(linearAcceleration instanceof Acceleration)
            || !(angularAccleration instanceof Acceleration)) {
            throw "linearAcceleration and angularAcceleration must be instances "
            + "of 'Acceleration'";
        }
        
        this.linearAcceleration = linearAcceleration;
        this.angularAccleration = angularAccleration;
    }
}

class Destroyable {
    constructor () {
        this.isDestroyed = false;
    }
    isDestroyed = false;
    onDestroyed () {
        for (var prop in this) {
            this[prop] = null;
        }
        this.isDestroyed = true;
    }
}

class Acceleration {
    constructor (xAcceleration, yAcceleration, zAcceleration) {
        this.xAcceleration = xAcceleration;
        this.yAcceleration = yAcceleration;
        this.zAcceleration = zAcceleration;
    }
}