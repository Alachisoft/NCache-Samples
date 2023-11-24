class Product:
    def __init__(self, id=None, name=None, unit_price=None, units_available=None):
        self.id = id
        self.name = name
        self.unit_price = unit_price
        self.units_available = units_available

    def __str__(self):
        return (f"\nID: {self.id}\nName: {self.name}\nUnit Price: {self.unit_price}\nUnits Available: {self.units_available}")
