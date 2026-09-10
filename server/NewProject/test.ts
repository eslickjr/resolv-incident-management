for (let i = 0; i < 10; i++) {
    if (i % 2 === 0) {
        console.log(i + " is even");
    } else {
        console.log(i + " is odd");
    }
}

class Animal {
    name: string;
    type: string;
    dogName: string;
    dogType: string;

    constructor(name: string, type: string) {
        this.name = name;
        this.type = type;
    }

    dogname = "buddy";
    dogtype = "squirrel";

    speak() {
        console.log(`${this.name} the ${this.type} says hello!`);
    }
    getInfo() {
        return `${this.name} is a ${this.type}`;
    }
}