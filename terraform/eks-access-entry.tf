resource "aws_eks_access_entry" "eks-access-entry-customer"{
    cluster_name = aws_eks_cluster.eks-cluster-customer.name
    principal_arn = var.principalArn
    kubernetes_group = ["customer-group-kubernetes"]
    type = "STANDARD"
}