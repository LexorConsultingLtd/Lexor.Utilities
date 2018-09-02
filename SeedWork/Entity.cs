using System;

namespace Utilities.SeedWork
{
    public abstract class Entity
    {
        private int? _requestedHashCode;

        public virtual int Id { get; protected set; }

        public bool IsTransient()
        {
            return Id == 0;
        }

        public abstract void CopyValues(Entity source);

        public override bool Equals(object obj)
        {
            if (!(obj is Entity))
                return false;

            if (ReferenceEquals(this, obj))
                return true;

            if (GetType() != obj.GetType())
                return false;

            var item = (Entity)obj;

            if (item.IsTransient() || IsTransient())
                return false;

            return item.Id == Id;
        }

        public override int GetHashCode()
        {
            if (!IsTransient())
            {
                // ReSharper disable NonReadonlyMemberInGetHashCode
                if (!_requestedHashCode.HasValue)
                    _requestedHashCode = Id.GetHashCode() ^ 31; // XOR for random distribution (http://blogs.msdn.com/b/ericlippert/archive/2011/02/28/guidelines-and-rules-for-gethashcode.aspx)

                return _requestedHashCode.Value;
                // ReSharper restore NonReadonlyMemberInGetHashCode
            }

            // ReSharper disable once BaseObjectGetHashCodeCallInGetHashCode
            return base.GetHashCode();
        }

        public static bool operator ==(Entity left, Entity right)
        {
            return left?.Equals(right) ?? Equals(right, null);
        }

        public static bool operator !=(Entity left, Entity right)
        {
            return !(left == right);
        }

        public T CheckEntityType<T>() where T : Entity
        {
            if (this is T result) return result;
            throw new ArgumentOutOfRangeException($"Entity type must be {typeof(T).Name}");
        }
    }
}
