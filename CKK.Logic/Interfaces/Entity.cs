using CKK.Logic.Exceptions;

namespace CKK.Logic.Interfaces
{
    [Serializable]
    public abstract class Entity
    {
        private int _id;
        public int Id
        {
            get { return _id; }
            set
            {
                if (value < 0)
                {
                    throw new InvalidIdException("New id is less than 0.");
                }
                _id = value;
            }
        }
        public string Name { get; set; }
    }
}
